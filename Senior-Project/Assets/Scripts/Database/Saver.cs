using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private Terrain ground;
    public GameObject groundPrefab;
    public TerrainData groundData;

    public GameObject treeOcculuderPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        //ground = Instantiate(groundPrefab, new Vector3(0,0,0), groundPrefab.transform.rotation).GetComponent<Terrain>();
        ground = GameObject.Find("Ground").GetComponent<Terrain>();
        
        //ground.terrainData = Instantiate(groundData);

        //ground = GetComponent<Terrain>();

        DBAccess.fixItQuick = ground.terrainData.treeInstances;

        if (DBAccess.isAReload()) {
            DBAccess.clearReload();

            DBAccess.startTransaction();

            List<Placeable> naturalObjects = DBAccess.getPlaceables();

            Vector3 size = ground.terrainData.size;

            naturalObjects.ForEach(placeable => 
            {
                if (placeable.getTileItemID() != (int)PlaceableTypes.Tree)
                    return;

                PathingManager.SetWalkable(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos()*ground.terrainData.size.x,0, ground.terrainData.size.x)), Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos()*ground.terrainData.size.z, 0, ground.terrainData.size.z)), false);
                Instantiate (treeOcculuderPrefab, new Vector3Int(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * ground.terrainData.size.x, 0, ground.terrainData.size.x)),0, Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * ground.terrainData.size.z, 0, ground.terrainData.size.z))), Quaternion.identity, ground.gameObject.transform);

                List<TreeInstance> tile = new();
                tile.AddRange(ground.terrainData.treeInstances);
                
                for(int i = 0; i < 10; i++)
                {
                    Vector3 position = new Vector3(placeable.getXPos(),0, placeable.getYPos());

                    tile.Add(new TreeInstance { 
                        prototypeIndex = 0, 
                        position = position, 
                        rotation = Random.Range(0,1), 
                        widthScale = 1, 
                        heightScale = 1
                    });
                }
                ground.terrainData.treeInstances = tile.ToArray();
                


            });
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
            if(tree.prototypeIndex == (int)PlaceableTypes.Tree)
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
        Tree,
        Stone
    }

}
