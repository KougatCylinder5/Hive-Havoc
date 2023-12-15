using System;
using System.Collections.Generic;
using UnityEngine;


public class MakeUnitBuilding : BuildingClass
{
    public int spawnCount;
    public GameObject[] unitToSpawn;
    public int buildingSize;
    private GetClickedObject gco;
    public int[] costOfUnit;
    public bool canMakeUnits = false;
    public LayerMask water;

    void Awake()
    {
        gco = GameObject.Find("ScriptManager").GetComponent<GetClickedObject>();
        if(Physics.BoxCast(transform.position, new(buildingSize*2, 2, buildingSize*2), Vector3.zero, transform.rotation, 0, water))
        {
            canMakeUnits = true;
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canMakeUnits)
        {
            GameObject go = gco.getBuilding();
            if (go.Equals(gameObject) && go.GetComponent<MakeUnitBuilding>() != null)
            {
                bool flag = true;
                for(int i = 0; i < ResourceStruct.Total.Length; i++)
                {
                    if(ResourceStruct.Total[i] < costOfUnit[i])
                    {
                        flag = false;
                        break;
                    }
                }
                if(flag)
                {
                    ResourceStruct.Wood -= costOfUnit[0];
                    ResourceStruct.Coal -= costOfUnit[1];
                    ResourceStruct.CopperOre -= costOfUnit[2];
                    ResourceStruct.CopperIngot -= costOfUnit[3];
                    ResourceStruct.Stone -= costOfUnit[4];
                    SpawnUnits(0);
                }
            }
        }
    }

    public void SpawnUnits(int unit)
    {
        List<Vector2Int> spots = new List<Vector2Int>();
        foreach(Vector2Int v in GenAllSurroundingSpawnPos())
        {
            if(PathingManager.ObstructedTiles[PathingManager.CalculateIndex(v.x, v.y)])
            {
                spots.Add(v);
            }
        }
        Vector2Int randVec = spots[UnityEngine.Random.Range(0, spots.Count)];
        Vector3 finalVec = new(randVec.x, 0, randVec.y);
        Saver.playerUnits.AddRange(MakeUnits.SpawnUnitsAtPosition(spawnCount, unitToSpawn[unit], transform.position + finalVec));
    }

    public List<Vector2Int> GenAllSurroundingSpawnPos()
    {
        List<Vector2Int> possiblePoints = new();
        for (int i = (int)Mathf.Pow(buildingSize + 2, 2) -1; i >= 0; --i)
        {
            Vector2Int point = new(2, 2);
            point += new Vector2Int(Mathf.FloorToInt(i % (buildingSize + 2)) - (int)Math.Round((float)(buildingSize / 2),MidpointRounding.AwayFromZero), Mathf.FloorToInt(i / (buildingSize + 2)) - (int)Math.Round((float)(buildingSize / 2), MidpointRounding.AwayFromZero));
            Debug.Log(point);
            if (PathingManager.IsOpen(point))
            {
                possiblePoints.Add(point);
            }
        }
        return possiblePoints;
    }
}
