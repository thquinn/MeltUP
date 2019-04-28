using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteScript : MonoBehaviour
{
    float originalX;

    // Start is called before the first frame update
    void Start()
    {
        originalX = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.x = (Camera.main.transform.localPosition.x - originalX) * .5f;
        pos.y = Camera.main.transform.localPosition.y;
        transform.localPosition = pos;
    }
}
