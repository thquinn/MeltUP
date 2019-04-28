using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    int frames;

    void Start() {
        transform.Rotate(0, 0, Random.Range(0f, 360f));
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = Color.HSVToRGB(Random.Range(.4125f, .4375f), Random.Range(.3f, .8f), Random.Range(.65f, .75f));
        color.a = 0;
        spriteRenderer.color = color;
    }

    void Update() {
        Color color = spriteRenderer.color;
        frames++;
        if (frames > 120) {
            color.a -= .02f;
            if (color.a <= 0) {
                Destroy(gameObject);
                return;
            }
        } else if (color.a < .75f) {
            color.a += .05f;
        }
        spriteRenderer.color = color;
        transform.Translate(0, .0005f, 0, Space.World);
        transform.Rotate(0, 0, .1f);
        transform.localScale += new Vector3(.01f, .01f);
    }
}
