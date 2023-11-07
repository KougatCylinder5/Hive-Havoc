using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

}
