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

    public static AudioScript Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null) {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this) {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
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
