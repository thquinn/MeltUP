using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneDeathScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float dx, dy = 0, dTheta;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dx = Random.Range(-.033f, -.02f);
        dTheta = Random.Range(-5f, 5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        Color color = spriteRenderer.color;
        color.a -= .025f;
        if (color.a <= 0) {
            Destroy(transform.parent.gameObject);
            return;
        }
        spriteRenderer.color = color;
        dy -= .002f;
        transform.Translate(dx, dy, 0, Space.World);
        transform.Rotate(0, 0, dTheta);
    }
}
