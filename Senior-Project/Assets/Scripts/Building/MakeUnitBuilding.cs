using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class MakeUnitBuilding : BuildingClass
{
    public int spawnCount;
    public GameObject unitToSpawn;
    public int buildingSize;
    private GetClickedObject gco;
    public int[] costOfUnit;

    void Awake()
    {
        health = maxHealth;
        gco = GameObject.Find("ScriptManager").GetComponent<GetClickedObject>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject go = gco.getBuilding();
            Debug.Log(go);
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
                    SpawnUnits();
                }
            }
        }
    }

    public void SpawnUnits()
    {
        bool onXSide = Random.value > 0.5f;
        float isPositive = Random.value - 0.5f;
        if(isPositive <= 0)
        {
            isPositive = -1;
        }
        else
        {
            isPositive = 1;
        }
        float x;
        float z;
        if(onXSide)
        {
            x = buildingSize * isPositive;
            z = (Random.value - 0.5f) * buildingSize;
        }
        else
        {
            z = buildingSize * isPositive;
            x = (Random.value - 0.5f) * buildingSize;
        }
        Vector3 randVec = new(x, transform.position.y, z);
        Saver.playerUnits.AddRange(MakeUnits.SpawnUnitsAtPosition(spawnCount, unitToSpawn, transform.position + randVec, transform));
    }
}
