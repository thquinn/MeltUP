using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    int frames;
    float angularVelocity;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
        angularVelocity = Random.Range(1f, 3f);
        if (Random.value < .5f) {
            angularVelocity *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        frames++;
        if (frames > 40) {
            color.a -= .05f;
            if (color.a <= 0) {
                Destroy(gameObject);
                return;
            }
        }
        else if (color.a < 1) {
            color.a += .1f;
        }
        spriteRenderer.color = color;
        transform.Rotate(0, 0, angularVelocity);
    }
}
