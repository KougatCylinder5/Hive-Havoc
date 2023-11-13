using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PathingManager;

public class SpawnTreeOccluders : MonoBehaviour
{

    private GameObject terrain;

    public GameObject treeOccluder;
    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.Find("Ground");
        Terrain groundComp = terrain.GetComponent<Terrain>();

        foreach (TreeInstance tree in groundComp.terrainData.treeInstances)
        {
            Instantiate(treeOccluder, Vector3.Scale(tree.position, groundComp.terrainData.size), Quaternion.identity, terrain.transform);
        }
        int size = groundComp.terrainData.alphamapResolution;
        for (int i = 0; i < Mathf.Pow(size,2); i++)
        {
            if (groundComp.terrainData.alphamapTextures[0].GetPixel(i / size, i % size).Equals(new(0, 0, 0, 1)))
            {
                ObstructedTiles[CalculateIndex(i / size * Mathf.CeilToInt(groundComp.terrainData.size.x)/size, i % size * Mathf.CeilToInt(groundComp.terrainData.size.z) / size)] = false;
            }
        }
    }

}
