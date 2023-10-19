using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MusicLooper : MonoBehaviour {
    private AudioSource musicSource;
    private AudioHighPassFilter musicHighPassFilter;
    public float musicLoopPoint;
    public float musicResetPoint;
    // Start is called before the first frame update
    void Start() {
        musicSource = GetComponent<AudioSource>();
        musicHighPassFilter = GetComponent<AudioHighPassFilter>();
        musicSource.Play();
    }

    // Update is called once per frame
    void Update() {
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