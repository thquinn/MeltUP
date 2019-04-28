using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrateScript : MonoBehaviour
{
    readonly int PUFF_PERIOD = 120;
    public GameObject puffPrefab;
    int offset;

    void Start() {
        offset = Random.Range(0, PUFF_PERIOD);
    }
    void Update()
    {
        if (Time.frameCount % PUFF_PERIOD == offset) {
            Instantiate(puffPrefab, transform);
        }
    }
}