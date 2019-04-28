using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackplateScript : MonoBehaviour
{
    public Vector3 originalPos;
    public float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = (Camera.main.transform.localPosition - originalPos) * multiplier;
        pos.z = originalPos.z;
        transform.localPosition = pos;
    }
}
