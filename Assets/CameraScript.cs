using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    readonly float TRACKING = .2f;

    public GameObject player;
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = transform.position.x * (1 - TRACKING) + player.transform.position.x * TRACKING;
        float y = transform.position.y * (1 - TRACKING) + player.transform.position.y * TRACKING;
        y = Mathf.Max(y, playerScript.deathY + 2.5f);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
