using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public float timeScale = 5;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(1,0,0) * Time.deltaTime * timeScale);
        //Debug.Log(gameObject.transform.eulerAngles.x);

        if(gameObject.transform.eulerAngles.x > 180) {
            Debug.Log(GetComponent<Light>().intensity);
            if(GetComponent<Light>().intensity > 100) {
                GetComponent<Light>().intensity -= (GetComponent<Light>().intensity * Time.deltaTime * timeScale);
            }
        } else {
            if (GetComponent<Light>().intensity < 81741.55f) {
                GetComponent<Light>().intensity += (10000 * Time.deltaTime * timeScale);
            }
        }
    }
}
