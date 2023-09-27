using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldGenerator : MonoBehaviour
{
    Vector2[,] flowTiles;
    Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = GameObject.FindGameObjectWithTag("CommandCenter").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
