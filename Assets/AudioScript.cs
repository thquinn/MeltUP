using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource bgm;
    float bgmVolume;
    public AudioSource[] jumps;
    public AudioSource[] uraniums;
    public AudioSource[] mutates;

    // Start is called before the first frame update
    void Start()
    {
        bgmVolume = bgm.volume;
        bgm.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (bgm.volume < bgmVolume) {
            bgm.volume = Mathf.Min(bgm.volume + bgmVolume / 100, bgmVolume);
        }
    }

    public void Jump() {
        jumps[Random.Range(0, jumps.Length)].Play();
    }

    public void Uranium() {
        uraniums[Random.Range(0, uraniums.Length)].Play();
    }

    public void Mutate() {
        mutates[Random.Range(0, mutates.Length)].Play();
    }
}
