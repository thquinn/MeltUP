using Assets.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    static float[] MAX_SPEED = new float[] { 1.25f, 2.125f, 3, 3.5f, 4};
    static readonly float FRICTION = .5f;
    static readonly float TEFLON_FRICTION = .99f;
    static readonly float TEFLON_CONTROL = .5f;
    static readonly float JUMP_POWER = 2.75f;
    static readonly float JUMP_GRAVITY = .33f;
    static float[] AIR_CONTROL = new float[] { 0, .15f, .55f, .75f, 1};
    static int JUMP_REFRESH_FRAMES = 3, GROUND_LINGER_FRAMES = 6;

    int LAYER_URANIUM, LAYER_PUFF;
    int LAYER_MASK_ENVIRONMENT;

    public GameObject spriteObject;
    Rigidbody2D rb2d;

    public List<Gene> genes;
    public int uranium = 0;
    public float mutationMeter = 1;
    int jumps = 0, jumpRefreshFrames = 0, groundLingerFrames = 0;
    int inPuffs = 0;
    bool rightLastInput = true;

    // Start is called before the first frame update
    void Start()
    {
        LAYER_URANIUM = LayerMask.NameToLayer("Uranium");
        LAYER_PUFF = LayerMask.NameToLayer("Puff");
        LAYER_MASK_ENVIRONMENT = 1 << LayerMask.NameToLayer("Environment");
        rb2d = GetComponent<Rigidbody2D>();

        genes = new List<Gene>();
        genes.Add(new Gene(GeneID.Speed));
        genes.Add(new Gene(GeneID.Speed));
        genes.Add(new Gene(GeneID.Jump));
        genes.Add(new Gene(GeneID.JumpControl));
        genes.Add(new Gene(GeneID.AirControl));
        genes.Add(new Gene(GeneID.AirControl));
        genes.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = CheckGrounded();
        TileType groundType = GroundType();

        float axis = Input.GetAxis("Horizontal");
        if (axis != 0) {
            rightLastInput = axis > 0;
        }
        float airControl = AirControl();
        if (!grounded) {
            axis *= airControl;
        }
        if (groundType == TileType.Teflon) {
            axis *= TEFLON_CONTROL;
        }
        float airFriction = Mathf.Lerp(1, FRICTION, airControl);
        float groundFriction = groundType == TileType.Teflon ? TEFLON_FRICTION : FRICTION;
        float friction = grounded ? groundFriction : airFriction;
        float newX = rb2d.velocity.x * friction;
        float maxSpeed = MaxSpeed();
        float accel = maxSpeed / 3;
        if (Mathf.Abs(rb2d.velocity.x) < maxSpeed) {
            newX = Mathf.Clamp(newX + axis * accel, -maxSpeed, maxSpeed);
        }

        float newY = rb2d.velocity.y;
        int maxJumps = MaxJumps();
        jumps = Mathf.Min(jumps, maxJumps);
        if (grounded && jumpRefreshFrames <= 0) {
            jumps = maxJumps;
            groundLingerFrames = GROUND_LINGER_FRAMES;
        } else if (!grounded) { 
            if (groundLingerFrames == 0) {
                jumps = Mathf.Min(maxJumps - 1, jumps, 0);
            } else {
                groundLingerFrames--;
            }
        }
        if (jumps > 0 && Input.GetButtonDown("Jump")) {
            newY = JUMP_POWER;
            jumps--;
            jumpRefreshFrames = JUMP_REFRESH_FRAMES;
        } else {
            jumpRefreshFrames--;
        }
        rb2d.velocity = new Vector2(newX, newY);
        spriteObject.transform.localRotation = Quaternion.Lerp(spriteObject.transform.localRotation, Quaternion.Euler(0, rightLastInput ? 0 : 180, 0), .33f);
        rb2d.gravityScale = newY > 0 && (Input.GetButton("Jump") || !HasGene(GeneID.JumpControl)) ? JUMP_GRAVITY : 1;

        mutationMeter += .00005f;
        mutationMeter += uranium * .00005f;
        mutationMeter += genes.Count * .00002f;
        if (inPuffs > 0) {
            mutationMeter += .005f;
        }
    }

    bool HasGene(GeneID id) {
        foreach (Gene gene in genes) {
            if (gene.id == id) {
                return true;
            }
        }
        return false;
    }
    int NumGene(GeneID id) {
        int ret = 0;
        foreach (Gene gene in genes) {
            if (gene.id == id) {
                ret++;
            }
        }
        return ret;
    }
    float MaxSpeed() {
        int speeds = NumGene(GeneID.Speed);
        speeds = Mathf.Min(speeds, MAX_SPEED.Length - 1);
        return MAX_SPEED[speeds];
    }
    int MaxJumps() {
        return Mathf.Min(NumGene(GeneID.Jump), 2);
    }
    bool CheckGrounded() {
        for (float dx = -.075f; dx <= .075f; dx += .075f) {
            Vector2 rayStart = new Vector3(transform.position.x + dx, transform.position.y);
            Debug.DrawLine(transform.position + new Vector3(dx, 0, 0), transform.position - new Vector3(-dx, .23f, 0));
            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, .23f, LAYER_MASK_ENVIRONMENT);
            if (hit.collider != null) {
                return true;
            }
        }
        return false;
    }
    TileType GroundType() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, .23f, LAYER_MASK_ENVIRONMENT);
        if (hit.collider == null) {
            return TileType.None;
        }
        if (hit.collider.gameObject.tag == "Teflon") {
            return TileType.Teflon;
        }
        return TileType.Ground;
    }
    float AirControl() {
        int airControls = NumGene(GeneID.AirControl);
        airControls = Mathf.Min(airControls, AIR_CONTROL.Length - 1);
        return AIR_CONTROL[airControls];
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LAYER_URANIUM) {
            Destroy(collision.gameObject);
            uranium++;
        }
        if (collision.gameObject.layer == LAYER_PUFF) {
            inPuffs++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LAYER_PUFF) {
            inPuffs--;
        }
    }
}
