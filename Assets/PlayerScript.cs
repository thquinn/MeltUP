using Assets.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    static float[] MAX_SPEED = new float[] { 3.1f, 3.6f, 4, 4.33f, 4.66f, 5};
    static readonly float FRICTION = .5f;
    static readonly float TEFLON_FRICTION = .99f;
    static readonly float TEFLON_CONTROL = .5f;
    static readonly float JUMP_POWER = 2.75f;
    static readonly float AIR_JUMP_POWER = 2.2f;
    static readonly float JUMP_POWER_BONUS = .275f;
    static readonly float JUMP_GRAVITY = .33f;
    static readonly float CONVEYOR_POWER = .025f;
    static readonly float SPRING_POWER = 7.5f;
    static float[] AIR_CONTROL = new float[] { .1f, .2f, .4f, .55f, .75f, 1};
    static int JUMP_REFRESH_FRAMES = 3, GROUND_LINGER_FRAMES = 6;
    static float GLIDE_SPEED = -.025f, GLIDE_POWER = .75f;
    static float BLINK_DISTANCE = 1;

    int LAYER_URANIUM, LAYER_PUFF, LAYER_CHECKPOINT, LAYER_SHOP_GENE;
    int LAYER_MASK_ENVIRONMENT;

    public GameObject spriteObject, airJumpPrefab;
    Rigidbody2D rb2d;

    public List<Gene> genes;
    public int uranium;
    public float mutationMeter;
    public int jumps;
    public float deathY;
    public TutorialScript tutorialScript;
    int jumpRefreshFrames, groundLingerFrames, blinks;
    bool inCheckpoint = false;
    int inPuffs = 0;
    bool rightLastInput = true;
    public Vector3 lastSafePosition;
    public float maxY;
    public bool dead;
    float lastConveyorImpulse;
    bool hitSpring;

    // Start is called before the first frame update
    void Start()
    {
        LAYER_URANIUM = LayerMask.NameToLayer("Uranium");
        LAYER_PUFF = LayerMask.NameToLayer("Puff");
        LAYER_CHECKPOINT = LayerMask.NameToLayer("Checkpoint");
        LAYER_SHOP_GENE = LayerMask.NameToLayer("ShopGene");
        LAYER_MASK_ENVIRONMENT = 1 << LayerMask.NameToLayer("Environment");
        rb2d = GetComponent<Rigidbody2D>();

        genes = new List<Gene>();
        genes.Add(new Gene(GeneID.JumpControl));
        genes.Add(new Gene(GeneID.AirControl));
        genes.Add(new Gene(GeneID.Respawn));

        deathY = -5f;
        lastSafePosition = transform.localPosition;
        uranium = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
        if (transform.localPosition.y > maxY) {
            maxY = transform.localPosition.y;
        }
        if (transform.localPosition.y < deathY) {
            if (HasGene(GeneID.Respawn)) {
                transform.localPosition = lastSafePosition;
            } else {
                dead = true;
            } 
        }
        deathY = Mathf.Max(deathY, transform.localPosition.y - 8);
        if (genes.Count == 0) {
            transform.Translate(0, -100, 0);
            dead = true;
        }
        if (dead) {
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            rb2d.velocity = Vector2.zero;
            return;
        }

        GroundResult groundResult = CheckGrounded();
        bool grounded = groundResult.num > 0;
        TileType groundType = groundResult.groundType;
        if (groundResult.num >= 7) {
            lastSafePosition = transform.localPosition;
        }

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
        if (grounded && jumpRefreshFrames <= 0) {
            jumps = MaxJumps();
            groundLingerFrames = GROUND_LINGER_FRAMES;
            blinks = 1;
            hitSpring = false;
        } else if (!grounded) { 
            if (groundLingerFrames == 0) {
                jumps = Mathf.Min(MaxPotentialJumps() - 1, jumps);
            } else {
                groundLingerFrames--;
            }
        }

        if (jumps > 0 && Input.GetButtonDown("Jump")) {
            float jumpPower = JUMP_POWER + NumGene(GeneID.JumpPower) * JUMP_POWER_BONUS;
            float airJumpPower = AIR_JUMP_POWER + NumGene(GeneID.JumpPower) * JUMP_POWER_BONUS;
            newY = Mathf.Max(newY, (grounded || groundLingerFrames > 0) ? jumpPower : airJumpPower);
            jumps--;
            jumpRefreshFrames = JUMP_REFRESH_FRAMES;
            GameObject fx = Instantiate(airJumpPrefab, transform.parent);
            fx.transform.localPosition = transform.localPosition + new Vector3(0, grounded ? -.2f : -.1f, 0);
            AudioScript.Instance.Jump();
        } else {
            jumpRefreshFrames--;
        }
        if (groundType == TileType.Conveyor) {
            transform.Translate(CONVEYOR_POWER, 0, 0);
            lastConveyorImpulse = CONVEYOR_POWER;
        } else if (groundType == TileType.ConveyorLeft) {
            transform.Translate(-CONVEYOR_POWER, 0, 0);
            lastConveyorImpulse = -CONVEYOR_POWER;
        } else if (groundType == TileType.Spring) {
            newY = Mathf.Max(newY, SPRING_POWER);
            hitSpring = true;
            jumpRefreshFrames = JUMP_REFRESH_FRAMES;
        } else if (lastConveyorImpulse != 0) {
            newX += lastConveyorImpulse * 60;
            lastConveyorImpulse = 0;
        }

        if (GetSpecialButton(GeneID.Glide) && newY < GLIDE_SPEED) {
            newY = Mathf.Lerp(newY, GLIDE_SPEED, GLIDE_POWER);
        }
        if (GetSpecialButtonDown(GeneID.Blink) && blinks > 0) {
            transform.Translate(BLINK_DISTANCE * (rightLastInput ? 1 : -1), 0, 0);
            blinks--;
        }

        // DEBUG STUFF
        if (Input.GetKey(KeyCode.F1) && Application.isEditor) {
            newY = 10;
        }

        rb2d.velocity = new Vector2(newX, newY);
        spriteObject.transform.localRotation = Quaternion.Lerp(spriteObject.transform.localRotation, Quaternion.Euler(0, rightLastInput ? 0 : 180, 0), .33f);
        rb2d.gravityScale = newY > 0 && (Input.GetButton("Jump") || !HasGene(GeneID.JumpControl)) && !hitSpring ? JUMP_GRAVITY : 1;

        bool tutorial = transform.localPosition.y < 2 && !tutorialScript.Done();
        if (!tutorial && !inCheckpoint) {
            mutationMeter += Mathf.Pow(Mathf.Max(0, maxY), 0.75f) * .0000125f;
            float uraniumMult = .00000175f * Mathf.Pow(.5f, NumGene(GeneID.UraniumBlock));
            mutationMeter += uranium * uraniumMult;
            mutationMeter += Mathf.Max(0, genes.Count - 4) * .00001f;
            if (inPuffs > 0) {
                mutationMeter += .005f;
            }
        }
    }

    public bool HasGene(GeneID id) {
        foreach (Gene gene in genes) {
            if (gene.id == id) {
                return true;
            }
        }
        return false;
    }
    public int NumGene(GeneID id) {
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
        return 1 + NumGene(GeneID.DoubleJump) + Random.Range(0, NumGene(GeneID.LeapOfFaith) + 1);
    }
    int MaxPotentialJumps() {
        return 1 + NumGene(GeneID.DoubleJump) + NumGene(GeneID.LeapOfFaith);
    }
    static readonly Dictionary<string, TileType> specialGroundTags = new Dictionary<string, TileType>() {
        { "Conveyor", TileType.Conveyor },
        { "ConveyorLeft", TileType.ConveyorLeft },
        { "Spring", TileType.Spring },
        { "Teflon", TileType.Teflon },
    };
    GroundResult CheckGrounded() {
        int ret = 0;
        TileType tileType = TileType.None;
        for (float dx = -.1f; dx <= .1f; dx += .025f) {
            Vector2 rayStart = new Vector3(transform.position.x + dx, transform.position.y);
            Debug.DrawLine(transform.position + new Vector3(dx, 0, 0), transform.position - new Vector3(-dx, .23f, 0));
            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, .23f, LAYER_MASK_ENVIRONMENT);
            if (hit.collider != null) {
                ret++;
                if (specialGroundTags.ContainsKey(hit.collider.gameObject.tag)) {
                    tileType = specialGroundTags[hit.collider.gameObject.tag];
                }
            }
        }
        if (tileType == TileType.None) {
            tileType = TileType.Ground;
        }
        return new GroundResult(ret, tileType);
    }
    float AirControl() {
        int airControls = NumGene(GeneID.AirControl);
        airControls = Mathf.Min(airControls, AIR_CONTROL.Length - 1);
        return AIR_CONTROL[airControls];
    }
    bool GetSpecialButton(GeneID id) {
        return HasGene(id) && Input.GetButton("Special");
    }
    bool GetSpecialButtonDown(GeneID id) {
        return HasGene(id) && Input.GetButtonDown("Special");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LAYER_URANIUM) {
            Destroy(collision.gameObject);
            uranium++;
            AudioScript.Instance.Uranium();
        }
        if (collision.gameObject.layer == LAYER_PUFF) {
            inPuffs++;
        }
        if (collision.gameObject.layer == LAYER_CHECKPOINT) {
            collision.gameObject.GetComponent<ShopSpawnerScript>().Enter(this);
            mutationMeter = Mathf.Max(0, mutationMeter - .33f * NumGene(GeneID.Restoration));
            inCheckpoint = true;
        }
        if (collision.gameObject.layer == LAYER_SHOP_GENE) {
            collision.gameObject.GetComponent<ShopGeneScript>().Buy(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LAYER_PUFF) {
            inPuffs--;
        }
        if (collision.gameObject.layer == LAYER_CHECKPOINT) {
            collision.gameObject.GetComponent<ShopSpawnerScript>().Leave(this);
            inCheckpoint = false;
        }
    }
}

struct GroundResult {
    public int num;
    public TileType groundType;

    public GroundResult(int num, TileType groundType) {
        this.num = num;
        this.groundType = groundType;
    }
}