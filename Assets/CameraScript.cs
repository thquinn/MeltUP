using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    readonly float TRACKING = .2f;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = transform.position.x * (1 - TRACKING) + player.transform.position.x * TRACKING;
        float y = transform.position.y * (1 - TRACKING) + player.transform.position.y * TRACKING;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
