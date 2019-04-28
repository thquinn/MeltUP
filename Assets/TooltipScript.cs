using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipScript : MonoBehaviour
{
    TextMeshPro tmp;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        Color color = tmp.color;
        color.a = 0;
        tmp.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = tmp.color;
        color.a = Mathf.Clamp01(Input.GetKey(KeyCode.LeftShift) ? color.a + .1f : color.a - .1f);
        tmp.color = color;
    }
}
