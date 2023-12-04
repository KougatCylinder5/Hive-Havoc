using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public float timeScale = 5;
    Light _sun;
    // Start is called before the first frame update
    void Start()
    {
        _sun = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(1,0,0) * Time.deltaTime * timeScale);

        if(gameObject.transform.eulerAngles.x > 180) {
            Debug.Log(_sun.intensity);
            if(_sun.intensity > 100) {
                _sun.intensity -= (_sun.intensity * Time.deltaTime * timeScale);
            }
        } else {
            if (_sun.intensity < 81741.55f) {
                _sun.intensity += (10000 * Time.deltaTime * timeScale);
            }
        }
    }
}
