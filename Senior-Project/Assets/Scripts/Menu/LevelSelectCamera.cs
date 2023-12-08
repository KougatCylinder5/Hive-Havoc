using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectCamera : MonoBehaviour
{
    public GameObject worldMap;
    public float scrollSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            worldMap.transform.position += new Vector3(0, -1 * scrollSpeed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            worldMap.transform.position += new Vector3(1 * scrollSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            worldMap.transform.position += new Vector3(0 , 1 * scrollSpeed, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            worldMap.transform.position += new Vector3(-1 * scrollSpeed, 0, 0);
        }
    }
}
