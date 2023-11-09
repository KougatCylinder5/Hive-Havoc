using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private Terrain ground;
    // Start is called before the first frame update
    void Start()
    {
        ground = GetComponent<Terrain>();

        if(true) {
            for(int i = 0; i < ground.terrainData.treeInstanceCount; i++) {
                ground.terrainData.treeInstances[i].position = new Vector3(0, 0, 0);
                ground.Flush();
            }
        }

        if (false) {
            DBAccess.fixItQuick = ground.terrainData.treeInstances;

            ground.terrainData.treeInstances = new TreeInstance[0];

            List<TreeInstance> newTrees = new List<TreeInstance>();
            foreach (Placeable tree in DBAccess.getPlaceables()) {

                if (tree.getTileItemID() == 0) {
                    TreeInstance newPos = new TreeInstance();
                    newPos.position = new Vector3(tree.getXPos(), 0, tree.getYPos());
                    newTrees.Add(newPos);
                }
            }

            ground.terrainData.treeInstances = newTrees.ToArray();

            ground.Flush();
        }
    }

    public void saveScene(string levelName) {
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
