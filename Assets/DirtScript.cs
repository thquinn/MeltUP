using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    new Collider2D collider2D;
    int timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        color.a = Mathf.Clamp01(color.a + (collider2D.enabled ? .166f : -.166f));
        spriteRenderer.color = color;

        if (timer > 0) {
            timer--;
            if (timer == 0) {
                collider2D.enabled = !collider2D.enabled;
                if (!collider2D.enabled) {
                    timer = 300;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (timer == 0) {
            timer = 24;
        }
    }
}
