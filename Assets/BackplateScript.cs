using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackplateScript : MonoBehaviour
{
    public Vector3 originalPos;
    public float multiplier;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = (cam.transform.localPosition - originalPos) * multiplier;
        pos.z = originalPos.z;
        transform.localPosition = pos;
    }
}
