using System.Collections.Generic;
using UnityEngine;

public class MakeUnits : MonoBehaviour
{
    //Spawn 'count' 'unit's at 'spawnPos', then return the list so the units can be saved
    public static List<GameObject> SpawnUnitsAtPosition(int count, GameObject unit, Vector3 spawnPos)
    {
        List<GameObject> spawnedUnits = new();

        for(int i = 0; i < count; i++)
        {
            spawnedUnits.Add(Instantiate(unit, spawnPos, Quaternion.identity));
        }
        return spawnedUnits;
    }
}