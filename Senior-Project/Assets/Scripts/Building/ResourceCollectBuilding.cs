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
    private Vector3 efficientVector;
    private Vector3 normalVector;
    private static Vector3 returnVector = new(4, 1, 4);

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        MakeUnits.SpawnUnitsAtPosition(spawnCount, unitToSpawn, transform.position);
        efficientVector = new(efficientRange, 1, efficientRange);
        normalVector = new(normalRange, 1, normalRange);
        returnVector = normalVector;
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
        if(Physics.CheckBox(transform.position, efficientVector, transform.rotation, LayerMask.GetMask("Trees")))
        {
            return 2;
        }
        else if(Physics.CheckBox(transform.position, normalVector, transform.rotation, LayerMask.GetMask("Trees")))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static Vector3 GetRange()
    {
        return returnVector;
    }
}
