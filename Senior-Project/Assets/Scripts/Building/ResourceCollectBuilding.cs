using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollectBuilding : MakeUnitBuilding
{
    public int woodPerSec;
    public int coalPerSec;
    public int copperOrePerSec;
    public int copperIngotPerSec;
    public int ironOrePerSec;
    public int ironIngotPerSec;
    public int steelPerSec;
    public int stonePerSec;
    private float timer = 1;
    private int reset = 1;
    public float efficientRange;
    public float normalRange;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        MakeUnits.SpawnUnitsAtPosition(spawnCount, unitToSpawn, transform.position, transform);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            AddResources(CalculateRange());
            timer = reset;
        }
    }

    public void AddResources(int multiplier)
    {
        ResourceStruct.Wood += woodPerSec * multiplier;
        ResourceStruct.Coal += coalPerSec * multiplier;
        ResourceStruct.CopperOre += copperOrePerSec * multiplier;
        ResourceStruct.CopperIngot += copperIngotPerSec * multiplier;
        ResourceStruct.IronOre += ironOrePerSec * multiplier;
        ResourceStruct.IronIngot += ironIngotPerSec * multiplier;
        ResourceStruct.Steel += steelPerSec * multiplier;
        ResourceStruct.Stone += stonePerSec * multiplier;
    }

    public int CalculateRange()
    {
        if(Physics.CheckSphere(transform.position, efficientRange, LayerMask.GetMask("Trees")))
        {
            return 2;
        }
        else if(Physics.CheckSphere(transform.position, normalRange, LayerMask.GetMask("Trees")))
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
}
