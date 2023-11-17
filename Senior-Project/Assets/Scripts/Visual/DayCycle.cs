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
        Debug.Log(gameObject.transform.rotation.x);
    }
}
