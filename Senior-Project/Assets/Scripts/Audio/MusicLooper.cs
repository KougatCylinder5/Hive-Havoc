using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MusicLooper : MonoBehaviour {
    private AudioSource musicSource;
    private AudioHighPassFilter musicHighPassFilter;
    public float musicLoopPoint;
    public float musicResetPoint;

    void Start() {
        musicSource = GetComponent<AudioSource>();
        musicHighPassFilter = GetComponent<AudioHighPassFilter>();
        musicSource.Play();
    }

    void Update() { //Loops the music from a end point and a reset point. Also addes an audio effect when the game is paused.
        if (musicSource.time >= musicResetPoint || musicSource.time == musicSource.clip.length) {
            musicSource.time -= musicResetPoint - musicLoopPoint;
            musicSource.Play();
        }

        if (Time.timeScale == 0) {
            if (musicHighPassFilter.cutoffFrequency < 2000) {
                musicHighPassFilter.cutoffFrequency += 5;
            }
        } else {
            try {
                if (musicHighPassFilter.cutoffFrequency > 10) {
                    musicHighPassFilter.cutoffFrequency -= 30;
                }
                if (musicHighPassFilter.cutoffFrequency < 10) {
                    musicHighPassFilter.cutoffFrequency = 10;
                }
            } catch {
                musicHighPassFilter.cutoffFrequency = 0;
            }
        }
    }
}