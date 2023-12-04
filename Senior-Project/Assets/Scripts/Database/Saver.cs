using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static DBAccess;
using static UnityEngine.UI.CanvasScaler;

public class Saver : MonoBehaviour
{
    private Terrain ground;
    public TerrainData groundData;

    public GameObject treeOcculuderPrefab;

    public Vector3 worldSize;

    public GameObject[] startingUnits;

    public static List<GameObject> resourceObstructers = new List<GameObject>();
    public static List<GameObject> playerUnits = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {

        playerUnits = startingUnits.ToList();
        ground = GameObject.Find("Ground").GetComponent<Terrain>();

        ground.terrainData = Instantiate(groundData);

        fixItQuick = ground.terrainData.treeInstances;

        if (!isAReload())
        {
            freshLoadScene();
        }
        else
        {
            foreach (GameObject unit in startingUnits) Destroy(unit);
        }
        startTransaction();

        List<Placeable> naturalObjects = getPlaceables();

        worldSize = groundData.size;

        ground.terrainData.treeInstances = new TreeInstance[0];

        naturalObjects.ForEach(placeable =>
        {
            GameObject occuluder = Instantiate(treeOcculuderPrefab, new Vector3Int(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), 0, Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z))), Quaternion.identity, ground.gameObject.transform);
            int amount = 0;
            resourceObstructers.Add(occuluder);
            switch (placeable.getTileItemID()) {
                case (int)PlaceableTypes.Tree:
                    PathingManager.SetWalkable(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z)), false);
                    occuluder.tag = "Tree Hitbox";
                    amount = 3;
                    break;
                case (int)PlaceableTypes.Stone:
                    occuluder.tag = "Stone Hitbox";
                    occuluder.GetComponent<MeshCollider>().enabled = false;
                    amount = 5;
                    break;
            }
            for (int i = 0; i < amount; i++)
            {
                Vector3 position = new Vector3(placeable.getXPos(), 0, placeable.getYPos());

                Vector2 randomOffset = UnityEngine.Random.onUnitSphere;

                position += new Vector3(randomOffset.x, 0, randomOffset.y) / 200;

                ground.AddTreeInstance(new TreeInstance
                {
                    prototypeIndex = placeable.getTileItemID(),
                    position = position,
                    rotation = UnityEngine.Random.Range(0, 360),
                    widthScale = 1,
                    heightScale = 1
                });
            }
        });
        ground.Flush();
        commitTransaction();
        startTransaction();
        List<Unit> units = getUnits();
        Debug.Log(units.Count);
        foreach (Unit unit in units)
        {
            GameObject tempHolder = Instantiate(Resources.Load(Enum.GetName(typeof(UnitTypes), (UnitTypes)unit.getType())) as GameObject, new Vector3(unit.getXPos(), 1, unit.getYPos()), Quaternion.identity);
            AIController controller = tempHolder.GetComponent<AIController>();
            controller.SetDestination(new Vector2(unit.getXTarget(), unit.getYTarget()));
            controller.Health = (int)unit.getHealth();
            
            playerUnits.Add(tempHolder);
        }
        commitTransaction();
        clearReload();
    }
    public void freshLoadScene()
    {
        startTransaction();
        groundData.treeInstances.ToList().ForEach(resource => { addPlaceable(resource.prototypeIndex, resource.position.x, resource.position.z, 1, 1); });
        foreach(GameObject unit in playerUnits)
        {
            AIController unitController = unit.GetComponent<AIController>();
            addUnit((int)Enum.Parse<UnitTypes>(unit.name[..unit.name.LastIndexOf('(')]), unitController.Position2D.x, unitController.Position2D.y, unitController.Target.x, unitController.Target.y, unitController.Health, 0);
        }
        commitTransaction();
    }
    public void saveScene()
    {
        startTransaction();
        clear();
        List<GameObject> invalidObstructers = new();

        foreach(GameObject blocker in resourceObstructers)
        {
            try
            {
                int type = -1;
                switch (blocker.tag)
                {
                    case "Tree Hitbox":
                        type = 0;
                        break;
                    case "Stone Hitbox":
                        type = 1;
                        break;
                }
                //Debug.Log(new Vector2(blocker.transform.position.x / worldSize.x, blocker.transform.position.z / worldSize.z));
                addPlaceable(type, blocker.transform.position.x / worldSize.x, blocker.transform.position.z / worldSize.z, 1, 1);
            }
            catch(Exception e) { invalidObstructers.Add(blocker); Debug.Log(e); }
        }
        foreach(GameObject invalid in invalidObstructers)
        {
            resourceObstructers.Remove(invalid);
        }

        for(int i = 0; i < playerUnits.Count; i++)
        {
            GameObject unit = playerUnits[i];
            try
            {
                AIController unitController = unit.GetComponent<AIController>();
                Debug.Log(unit.name[..unit.name.LastIndexOf('(')]);
                addUnit((int)Enum.Parse<UnitTypes>(unit.name[..unit.name.LastIndexOf('(')]), unitController.Position2D.x, unitController.Position2D.y, unitController.Target.x, unitController.Target.y, unitController.Health, 0);
            }
            catch { playerUnits.RemoveAt(i--); }
        }        

        commitTransaction();
    }

    public void OnApplicationQuit()
    {
        saveScene();
    }

    public enum PlaceableTypes
    {
        Tree,
        Stone
    }
    //This must match the prefab name
    public enum UnitTypes
    {
        Hunter,
        Crawler
    }

}
