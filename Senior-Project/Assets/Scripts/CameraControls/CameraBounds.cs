using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public GameObject bound1;
    public GameObject bound2;
    public GameObject bound3;
    public GameObject bound4;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public Vector3 CheckBounds(Vector3 position)
    {
        return new Vector3(Mathf.Clamp(position.x, bound4.transform.position.x, bound2.transform.position.x), position.y, Mathf.Clamp(position.z, bound3.transform.position.z, bound1.transform.position.z));
    }
}
