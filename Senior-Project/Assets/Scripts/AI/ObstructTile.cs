using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
* Attaches to a game object that when spawned blocks movement on its current location
*/
public class ObstructTile : MonoBehaviour
{
    void Awake()
    {
        PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z), PathingManager.GridSize.x)] = false;
    }

    private void OnDestroy()
    {
        PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z), PathingManager.GridSize.x)] = true;
    }
}
