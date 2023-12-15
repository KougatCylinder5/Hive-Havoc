using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffect : MonoBehaviour
{
    [SerializeField]
    private float xDirection = 0.1f;
    [SerializeField]
    private float yDirection = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        if(UnityEngine.Random.Range(0, 2) == 1) {
            xDirection = -xDirection;
        }
        if (UnityEngine.Random.Range(0, 2) == 1) {
            yDirection = -yDirection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.x > 5) {
            xDirection = -0.1f;
        }
        if (transform.localPosition.x < -5) {
            xDirection = 0.1f;
        }
        if (transform.localPosition.y > 2) {
            yDirection = -0.1f;
        }
        if (transform.localPosition.y < -2) {
            yDirection = 0.1f;
        }

        transform.localPosition = transform.localPosition + new Vector3(xDirection, yDirection, 0) * Time.deltaTime;
    }
}
