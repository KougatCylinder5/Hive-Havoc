using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLooper : MonoBehaviour
{
    private AudioSource musicSource;
    public AudioClip musicClip;
    public float musicLoopPoint;
    public float musicResetPoint;
    // Start is called before the first frame update
    void Start() {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update() {
        if(musicSource.time >= musicResetPoint || musicSource.time == musicClip.length) {
            musicSource.time = musicLoopPoint;
            musicSource.Play();
        }
    }
}
