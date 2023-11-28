using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private Terrain ground;
    public TerrainData groundData;

    public GameObject treeOcculuderPrefab;


    public Vector3Int worldSize;
    // Start is called before the first frame update
    void Awake()
    {
        //ground = Instantiate(groundPrefab, new Vector3(0,0,0), groundPrefab.transform.rotation).GetComponent<Terrain>();
        ground = GameObject.Find("Ground").GetComponent<Terrain>();

        ground.terrainData = Instantiate(groundData);

        DBAccess.fixItQuick = ground.terrainData.treeInstances;

        if (!DBAccess.isAReload())
        {
            saveScene();
        }
        DBAccess.clearReload();
        DBAccess.startTransaction();

        List<Placeable> naturalObjects = DBAccess.getPlaceables();

        Vector3 worldSize = ground.terrainData.size;

        ground.terrainData.treeInstances = new TreeInstance[0];

        naturalObjects.ForEach(placeable =>
        {
            if (placeable.getTileItemID() == (int)PlaceableTypes.Tree) {
                PathingManager.SetWalkable(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z)), false);
                GameObject occuluder = Instantiate(treeOcculuderPrefab, new Vector3Int(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), 0, Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z))), Quaternion.identity, ground.gameObject.transform);
                occuluder.tag = "Tree Hitbox";
            }

            int amount = 0;

            switch (placeable.getTileItemID())
            {
                case 0:
                    amount = 3;
                    break;
                case 1:
                    amount = 1;
                    break;
            }





            for (int i = 0; i < amount; i++)
            {
                Vector3 position = new Vector3(placeable.getXPos(), 0, placeable.getYPos());

                Vector2 randomOffset = Random.onUnitSphere;

                position += new Vector3(randomOffset.x, 0, randomOffset.y) / 200;

                ground.AddTreeInstance(new TreeInstance
                {
                    prototypeIndex = placeable.getTileItemID(),
                    position = position,
                    rotation = Random.Range(0, 360),
                    widthScale = 1,
                    heightScale = 1
                });
            }




        });
        ground.Flush();
        DBAccess.commitTransaction();
    }

    public void saveScene()
    {
        DBAccess.startTransaction();
        DBAccess.clear();

        groundData.treeInstances.ToList().ForEach(tree => {DBAccess.addPlaceable(tree.prototypeIndex, tree.position.x, tree.position.z, 1, 1); });

        DBAccess.commitTransaction();
    }

    public void quickFix()
    {
        ground.terrainData.treeInstances = DBAccess.fixItQuick;
        ground.Flush();
    }

    public enum PlaceableTypes
    {
        Tree,
        Stone
    }

}
