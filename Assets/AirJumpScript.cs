using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJumpScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(.1f, .1f);
        Color color = spriteRenderer.color;
        color.a -= .1f;
        if (color.a <= 0) {
            Destroy(gameObject);
            return;
        }
        spriteRenderer.color = color;
    }
}
