using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour {
    public float timeScale = 1;
    public float cycleTime = 600;
    private float sunrise = 0;
    private float sunset;
    private float noon;
    private float night;
    public float currentTime = 0;

    Light _sun;
 
    void Start() {
        _sun = GetComponent<Light>();
        float cycleDurr = cycleTime / 4;
        noon = cycleDurr;
        sunset = cycleDurr * 2;
        night = cycleDurr * 3;
    }

    void Update() { //Jumps between different phases of the day.
        currentTime += Time.deltaTime;
        if(currentTime >= cycleTime) {
            currentTime -= cycleTime;
            //Auto save the game.
        } else if(currentTime >= night) {
            if(_sun.intensity > 200) {
                _sun.intensity /= 2;
                gameObject.transform.rotation = Quaternion.Euler(90, -30, 0);
            }
        } else if(currentTime >= sunset) {
            gameObject.transform.rotation = Quaternion.Euler(160, -30, 0);
        } else if(currentTime >= noon) {
            gameObject.transform.rotation = Quaternion.Euler(90, -30, 0);
        } else if(currentTime >= sunrise) {
            if (_sun.intensity < 5000) {
                _sun.intensity *= 2;
                gameObject.transform.rotation = Quaternion.Euler(30, -30, 0);
            }
        }
    }
}
