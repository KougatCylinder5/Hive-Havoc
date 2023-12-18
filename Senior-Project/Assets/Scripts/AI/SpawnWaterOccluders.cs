using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PathingManager;

public class SpawnWaterOccluders : MonoBehaviour
{

    private GameObject terrain;

    public GameObject occluder;
    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.Find("Ground");
        Terrain groundComp = terrain.GetComponent<Terrain>();
        int size = groundComp.terrainData.alphamapResolution;
        for (int i = 0; i < Mathf.Pow(size,2); i += Mathf.RoundToInt(size/groundComp.terrainData.size.x * 4.44f))
        {
            if (groundComp.terrainData.alphamapTextures[0].GetPixel(i / size, i % size).Equals(new(0, 0, 0, 1)))
            {
                ObstructedTiles[CalculateIndex(i / size * Mathf.RoundToInt(groundComp.terrainData.size.x) / size, i % size * Mathf.RoundToInt(groundComp.terrainData.size.z) / size)] = false;
                Instantiate(occluder, new(i / size * Mathf.RoundToInt(groundComp.terrainData.size.x) / size, 0.1f, i % size * Mathf.RoundToInt(groundComp.terrainData.size.z) / size), Quaternion.identity, terrain.transform).layer = LayerMask.NameToLayer("Water");
            }
        }
    }

}
