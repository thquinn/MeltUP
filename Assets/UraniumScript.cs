using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UraniumScript : MonoBehaviour
{
    readonly int SPARKLE_PERIOD = 30;

    public GameObject sparklePrefab;

    Vector3 localPosition;

    // Start is called before the first frame update
    void Start()
    {
        localPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.frameCount * .033f) * .015f;
        transform.localPosition = localPosition + new Vector3(0, y, 0);

        if (Time.frameCount % SPARKLE_PERIOD == 0) {
            GameObject sparkle = Instantiate(sparklePrefab, transform);
            float angle = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(0f, .075f);
            sparkle.transform.Translate(r * Mathf.Cos(angle), r * Mathf.Sin(angle), 0);
        }
    }
}
