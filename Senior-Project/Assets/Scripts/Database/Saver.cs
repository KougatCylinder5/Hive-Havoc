using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private Terrain ground;
    public GameObject groundPrefab;
    public TerrainData groundData;
    // Start is called before the first frame update
    void Awake()
    {
        //ground = Instantiate(groundPrefab, new Vector3(0,0,0), groundPrefab.transform.rotation).GetComponent<Terrain>();
        ground = GameObject.Find("Ground").GetComponent<Terrain>();
        ground.terrainData = Instantiate(groundData);

        //ground = GetComponent<Terrain>();

        DBAccess.fixItQuick = ground.terrainData.treeInstances;

        if (DBAccess.isAReload()) {
            DBAccess.clearReload();

            DBAccess.startTransaction();

            List<Placeable> naturalObjects = DBAccess.getPlaceables();

            naturalObjects.ForEach(placeable => 
            {
                if (placeable.getTileItemID() != (int)PlaceableTypes.Tree)
                    return;

                PathingManager.SetWalkable(Mathf.RoundToInt(placeable.getXPos()), Mathf.RoundToInt(placeable.getYPos()), false);
                Debug.Log(placeable);
                

            });
 
            
            

            DBAccess.commitTransaction();
        } else {
            saveScene();
        }
    }

    public void saveScene() {
        DBAccess.startTransaction();
        DBAccess.clear();

        foreach (TreeInstance tree in ground.terrainData.treeInstances) {
            DBAccess.addPlaceable(0, tree.position.x, tree.position.z, 1, 1);
        }

        DBAccess.commitTransaction();
    }

    public void quickFix() {
        ground.terrainData.treeInstances = DBAccess.fixItQuick;
        ground.Flush();
    }

    public enum PlaceableTypes
    {
        Tree
    }

}
