using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceCollectBuilding : BuildingClass
{
    public int resourcePerSec;
    private int reset = 1;
    public float efficientRange;
    public float normalRange;
    public GatheringType type;
    public Terrain terrain;
    private float distance;

    int multiplier;

    // Start is called before the first frame update
    void Awake()
    {
        //MakeUnits.SpawnUnitsAtPosition(spawnCount, unitToSpawn, transform.position, transform);
        terrain = GameObject.Find("Ground").GetComponent<Terrain>();
        multiplier = CalculateRange();
        
        InvokeRepeating(nameof(AddResources), 0, reset);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddResources()
    {
        switch(type)
        {
            case 0:
                ResourceStruct.Wood += resourcePerSec * multiplier;
                break;
            case (GatheringType)1:
                ResourceStruct.Stone += resourcePerSec * multiplier;
                break;
            case (GatheringType)2:
                ResourceStruct.Coal += resourcePerSec * multiplier;
                break;
            case (GatheringType)3:
                ResourceStruct.CopperOre += resourcePerSec * multiplier;
                break;
            case (GatheringType)4:
                ResourceStruct.CopperIngot += resourcePerSec * multiplier;
                break;
        }
    }

    public int CalculateRange()
    {
        TreeInstance nearestTree = terrain.terrainData.treeInstances.OrderBy(tree => 
        { 
            return tree.prototypeIndex == (int)type 
            ? 
            Vector2.Distance(
                new(transform.position.x, transform.position.z), 
                new(tree.position.x * terrain.terrainData.size.x, tree.position.z * terrain.terrainData.size.z)) 
            : 
                float.PositiveInfinity;
        }).ToArray()[0];
        distance = Vector3.Distance(transform.position, Vector3.Scale(nearestTree.position,terrain.terrainData.size));
        if(distance <= efficientRange)
        {
            return 2;
        }
        else if(distance <= normalRange)
        {
            return 1;
        }
        else
        {
            return 0; 
        }
    }

    public float GetRange()
    {
        return normalRange;
    }

    public enum GatheringType
    {
        Wood,
        Stone,
        Coal,
        CopperOre,
        CopperIngot
        
    }

}
