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
    private int rate;
    int multiplier;

    void Awake()
    {
        //When this object is placed, detect how close it is to its resource, then collect resources every cycle until destroyed.
        terrain = GameObject.Find("Ground").GetComponent<Terrain>();
        multiplier = CalculateRange();
        InvokeRepeating(nameof(AddResources), 0, reset);
    }

    public int getRate() {
        return rate;
    }
    //Give the player resources
    public void AddResources()
    {
        rate = resourcePerSec * multiplier;
        switch (type)
        {
            case GatheringType.Wood:
                ResourceStruct.Wood += rate;
                break;
            case GatheringType.Stone:
                ResourceStruct.Stone += rate;
                break;
            case GatheringType.Coal:
                ResourceStruct.Coal += rate;
                break;
            case GatheringType.CopperOre:
                ResourceStruct.CopperOre += rate;
                break;
            case GatheringType.CopperIngot:
                ResourceStruct.CopperIngot += rate;
                break;
        }
    }

    public int CalculateRange()
    {
        //Return a mulitplier based on how close the building is to the resource it is collecting.
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

    public enum GatheringType //has to match treeprototypeindex in terrains
    {
        Wood = 0,
        Stone = 1,
        Coal = 3,
        CopperOre = 2,
        CopperIngot = 4
    }
}