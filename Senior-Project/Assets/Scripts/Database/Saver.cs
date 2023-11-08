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
        saveScene("test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveScene(string levelName) {
        DBAccess.startTransaction();
        DBAccess.clear();

        foreach (TreeInstance tree in ground.terrainData.treeInstances) {
            DBAccess.addPlaceable(0, tree.position.x, tree.position.z, 1, 1);
        }

        DBAccess.commitTransaction();
    }
}
