using System.Collections.Generic;
using UnityEngine;

public class MakeUnits : MonoBehaviour
{
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