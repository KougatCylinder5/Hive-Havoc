using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class MakeUnitBuilding : BuildingClass
{
    public int spawnCount;
    public GameObject unitToSpawn;

    void Awake()
    {
        health = maxHealth;
        MakeUnits.SpawnUnitsAtPosition(spawnCount, unitToSpawn, transform.position, transform);
    }
}
