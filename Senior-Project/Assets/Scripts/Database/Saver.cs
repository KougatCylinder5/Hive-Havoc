using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static DBAccess;

public class Saver : MonoBehaviour
{
    private Terrain ground;
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
            List<TreeInstance> newTrees = new List<TreeInstance>();
            int index = 0;
 
            foreach (Placeable tree in DBAccess.getPlaceables()) {
                
                if (tree.getTileItemID() == 0) {
                    TreeInstance newPos = ground.terrainData.treeInstances[index];
                    newPos.position = new Vector3(tree.getXPos(), 0, tree.getYPos());
                    newTrees.Add(newPos);
                }
                index++;
            }

            ground.terrainData.treeInstances = newTrees.ToArray();

            ground.Flush();

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


}
