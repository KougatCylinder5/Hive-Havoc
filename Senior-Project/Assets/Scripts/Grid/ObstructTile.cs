using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PathingManager.ObstructedTiles[PathingManager.CalculateIndex((int)(transform.position.x), (int)(transform.position.z), PathingManager.GridSize.x)] = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
