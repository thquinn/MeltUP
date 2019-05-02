using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteScript : MonoBehaviour
{
    Camera cam;
    float originalX;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        originalX = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.x = (cam.transform.localPosition.x - originalX) * .5f;
        pos.y = cam.transform.localPosition.y;
        transform.localPosition = pos;
    }
}
