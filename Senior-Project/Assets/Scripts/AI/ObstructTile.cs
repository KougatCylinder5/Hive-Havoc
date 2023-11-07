using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z), PathingManager.GridSize.x)] = false;
    }

    private void OnDestroy()
    {
        PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z), PathingManager.GridSize.x)] = true;
    }
}
