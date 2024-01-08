using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using static DBAccess;
using static UnityEngine.UI.CanvasScaler;

public class Saver : MonoBehaviour
{
    private Terrain ground;
    public TerrainData groundData;

    public GameObject treeOcculuderPrefab;

    public static Vector3 worldSize;

    public GameObject[] startingUnits;
    public GameObject[] startingBuildings;

    public static List<GameObject> resourceObstructers = new List<GameObject>();
    public static List<GameObject> allUnits = new List<GameObject>();
    public static List<GameObject> allBuildings = new List<GameObject>();

    public int startingWood = 0;
    public int startingStone = 0;
    public int startingIngot = 0;
    public int startingCoal = 0;
    public int startingOre = 0;

    public static bool LoadDone { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        // Give enough resources to start production of more
        // will be overriden if loading from a prior save
        LoadDone = false;
        ResourceStruct.Wood = startingWood;
        ResourceStruct.Stone = startingStone;
        ResourceStruct.Coal = startingCoal;
        ResourceStruct.CopperIngot = startingIngot;
        ResourceStruct.CopperOre = startingOre;
        Invoke(nameof(Load), 0.5f);
    }
    void Load()
    {
        // start the coroutine to prevent the game from freezing while loading
        StartCoroutine(nameof(LoadSave));
    }
    public IEnumerator LoadSave()
    {
        // get starting units 
        allUnits = startingUnits.ToList();
        allBuildings = startingBuildings.ToList();
        ground = GameObject.Find("Ground").GetComponent<Terrain>();
        // create a copy of the ground so as not to overwrite the existing data
        ground.terrainData = Instantiate(groundData);
        int counter = 0;
        // saves all the existing things to the DB for later
        if (!isAReload())
        {
            startTransaction();
            List<Placeable> allResources = new();
            // iterate over every treeInstance (includes rocks and ore that can be harvested) to save its location in the db 
            foreach (TreeInstance resource in groundData.treeInstances.ToList())
            {   
                
                allResources.Add(new(0, resource.prototypeIndex, resource.position.x, resource.position.z, 1, 1));
                // yield to allow the next frame
                if (counter++ > 20)
                {
                    counter = 0;
                    yield return 0;
                }
            }
            // add all at once as it is quicker than at once
            addPlaceables(allResources);
            
            // all starting structures (enemy and player alike)
            List<Placeable> allBuildingsPlaceable = new();
            foreach (GameObject b in allBuildings)
            {
                allBuildingsPlaceable.Add(new(0, (int)Enum.Parse<PlaceableTypes>(b.name[..b.name.IndexOf('(')]), b.transform.position.x, b.transform.position.z, b.GetComponent<IHealth>().Health, 0));
                if (counter++ > 20)
                {
                    counter = 0;
                    yield return 0;
                }
            }
            addPlaceables(allBuildingsPlaceable);

            // all starting units (enemy and player)
            foreach (GameObject unit in allUnits)
            {
                AIController unitController = unit.GetComponent<AIController>();
                addUnit((int)Enum.Parse<UnitTypes>(unit.name[..unit.name.IndexOf('(')]), unitController.Position2D.x, unitController.Position2D.y, unitController.Target.x, unitController.Target.y, unitController.Health, 0);
                if (counter++ > 25)
                {
                    counter = 0;
                    yield return 0;
                }
            }
            // set with default values
            updateInventory((int)ItemsID.Wood, ResourceStruct.Wood);
            updateInventory((int)ItemsID.Coal, ResourceStruct.Coal);
            updateInventory((int)ItemsID.CopperOre, ResourceStruct.CopperOre);
            updateInventory((int)ItemsID.CopperIngot, ResourceStruct.CopperIngot);
            updateInventory((int)ItemsID.Stone, ResourceStruct.Stone);
            commitTransaction();
        }
        foreach (GameObject unit in startingUnits) Destroy(unit);
        foreach (GameObject building in startingBuildings) Destroy(building);
        // removes all things and reinstantiates them so to have a fresh slate
        startTransaction();

        // set them from the DB
        ResourceStruct.Wood = amountInInventory((int)ItemsID.Wood);
        ResourceStruct.Coal = amountInInventory((int)ItemsID.Coal);
        ResourceStruct.CopperOre = amountInInventory((int)ItemsID.CopperOre);
        ResourceStruct.CopperIngot = amountInInventory((int)ItemsID.CopperIngot);
        ResourceStruct.Stone = amountInInventory((int)ItemsID.Stone);

        commitTransaction();
        startTransaction();

        // gets all the trees and rocks for placement
        List<Placeable> naturalObjects = getNaturalPlaceables();

        worldSize = groundData.size;

        ground.terrainData.treeInstances = new TreeInstance[0];
        ground.Flush();
        counter = 0;
        foreach (Placeable placeable in naturalObjects)
        {
            counter++;
            GameObject occuluder = Instantiate(treeOcculuderPrefab, new Vector3Int(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), 0, Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z))), Quaternion.identity, ground.gameObject.transform);
            int amount = 0;
            resourceObstructers.Add(occuluder);
            // spawn a different object depending upon what type it is from the DB
            switch (placeable.getTileItemID())
            {
                case (int)PlaceableTypes.Tree:
                    PathingManager.SetWalkable(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z)), false);
                    occuluder.tag = "Tree Hitbox";
                    amount = 2;
                    break;
                case (int)PlaceableTypes.Stone:
                    PathingManager.SetWalkable(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z)), false);
                    occuluder.tag = "Stone Hitbox";
                    amount = 2;
                    break;
                case (int)PlaceableTypes.Coal:
                    PathingManager.SetWalkable(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z)), false);
                    occuluder.tag = "Coal Hitbox";
                    amount = 2;
                    break;
                case (int)PlaceableTypes.Copper:
                    PathingManager.SetWalkable(Mathf.RoundToInt(Mathf.Clamp(placeable.getXPos() * worldSize.x, 0, worldSize.x)), Mathf.RoundToInt(Mathf.Clamp(placeable.getYPos() * worldSize.z, 0, worldSize.z)), false);
                    occuluder.tag = "Copper Hitbox";
                    amount = 2;
                    break;

            }
            // amount to spawn in an area so that the tiles look full and aren't just one tree/rock
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
            // can't forget to yield for frame
            if (counter > 15)
            {
                counter = 0;
                yield return 0;
            }
        }
        // update the ground so the rocks/ trees are properly displayed
        ground.Flush();
        commitTransaction();
        startTransaction();
        // spawning all units from DB
        List<Unit> units = getUnits();
        foreach (Unit unit in units)
        {
            GameObject tempHolder = Instantiate(Resources.Load(Enum.GetName(typeof(UnitTypes), (UnitTypes)unit.getType())) as GameObject, new Vector3(unit.getXPos(), 0.5f, unit.getYPos()), Quaternion.identity);
            AIController controller = tempHolder.GetComponent<AIController>();
            // assign health and target from memory for a smooth experience
            controller.SetDestination(new Vector2(unit.getXTarget(), unit.getYTarget()));
            controller.SetHealth((int)unit.getHealth());

            allUnits.Add(tempHolder);
            //can't forget to yield for frame
            if (counter > 15)
            {
                counter = 0;
                yield return 0;
            }
        }
        // get all buildings
        List<Placeable> buildings = getPlaceables();
        allBuildings.Clear();
        foreach (Placeable building in buildings)
        {
            if (building.getTileItemID() <= 3)
                continue;
            GameObject tempHolder = Instantiate(Resources.Load(Enum.GetName(typeof(PlaceableTypes), (PlaceableTypes)building.getTileItemID())) as GameObject, new Vector3(building.getXPos(), 0.5f, building.getYPos()), Quaternion.identity);
            allBuildings.Add(tempHolder);
            tempHolder.GetComponent<IHealth>().SetHealth((int)building.getHealth());
            
            // some buildings are required for win/loss conditions and as such must be assigned elsewhere to be checked against

            if((PlaceableTypes)building.getTileItemID() == PlaceableTypes.CommandCenter)
            {
                
                WinLoseCondition.commandCenter = tempHolder;
            }
            else if((PlaceableTypes)building.getTileItemID() == PlaceableTypes.Nest)
            {
                WinLoseCondition.nests.Add(tempHolder);
            }
            // can't forget to yield
            if (counter > 15)
            {
                counter = 0;
                yield return 0;
            }
        }

        commitTransaction();
        clearReload();
        LoadDone = true;
    }
    /**
     * Called by ApplicationQuit() to save upon exit or crash
     */
    public static void saveScene()
    {
        // if the load was never complete do not save to prevent a partial scene save
        if(!LoadDone) return;
        startTransaction();
        clear();
        // objects may be removed or somehow become invalid 
        // don't want them to be saved in this state so removing them
        List<GameObject> invalidThings = new();
        List<Placeable> placeables = new();
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
                    case "Coal Hitbox":
                        type = 2;
                        break;
                    case "Copper Hitbox":
                        type = 3;
                        break;

                }
                placeables.Add(new Placeable(0,type, blocker.transform.position.x / worldSize.x, blocker.transform.position.z / worldSize.z, 1, 1));
            }
            catch(Exception e) { invalidThings.Add(blocker); Debug.Log(e); }
        }
        foreach(GameObject invalid in invalidThings)
        {
            resourceObstructers.Remove(invalid);
        }
        foreach (GameObject building in allBuildings)
        {
            try
            {
                BuildingClass buildingClass = building.GetComponent<BuildingClass>();
                placeables.Add(new(0, (int)Enum.Parse<PlaceableTypes>(building.name[..building.name.IndexOf('(')]), building.transform.position.x, building.transform.position.z, buildingClass.Health, 0));
                if (Enum.Parse<PlaceableTypes>(building.name[..building.name.IndexOf('(')]) == PlaceableTypes.Nest)
                {
                    WinLoseCondition.nests.Add(building);
                }
                else if(Enum.Parse<PlaceableTypes>(building.name[..building.name.IndexOf('(')]) == PlaceableTypes.CommandCenter)
                {
                    WinLoseCondition.commandCenter = building;
                }
            }
            catch { invalidThings.Add(building); }
        }
        foreach (GameObject invalid in invalidThings)
        {
            allBuildings.Remove(invalid);
        }
        invalidThings.Clear();
        addPlaceables(placeables);
        foreach(GameObject unit in allUnits)
        {
            try
            {
                AIController unitController = unit.GetComponent<AIController>();
                addUnit((int)Enum.Parse<UnitTypes>(unit.name[..unit.name.IndexOf('(')]), unitController.Position2D.x, unitController.Position2D.y, unitController.Target.x, unitController.Target.y, unitController.Health, 0);
            }
            catch { invalidThings.Add(unit); }
        }
        foreach (GameObject invalid in invalidThings)
        {
            allUnits.Remove(invalid);
        }
        

        // update resources in inventory
        updateInventory((int)ItemsID.Wood, ResourceStruct.Wood);
        updateInventory((int)ItemsID.Coal, ResourceStruct.Coal);
        updateInventory((int)ItemsID.CopperOre, ResourceStruct.CopperOre);
        updateInventory((int)ItemsID.CopperIngot, ResourceStruct.CopperIngot);
        updateInventory((int)ItemsID.Stone, ResourceStruct.Stone);

        commitTransaction();
    }

    public void OnApplicationQuit()
    {
        saveScene();
    }
    // enum so that all types of objects are in sync across all scripts
    public enum PlaceableTypes
    { 
        Tree,
        Stone,
        Coal,
        Copper,
        Tent,
        WoodHouse,
        StoneHouse,
        CoalHut,
        WoodHut,
        CopperHut,
        WoodWall,
        StoneWall,
        CartMaker,
        Ballista,
        Nest,
        CommandCenter


    }
    //This must match the prefab name
    public enum UnitTypes
    {
        DrillCart,
        Crawler
    }

    // resource IDs synced across scripts
    private enum ItemsID {
        Wood,
        Coal,
        CopperOre,
        CopperIngot,
        Stone
    }

}
