using UnityEngine;

public class MakeUnits : MonoBehaviour
{
    public static void SpawnUnitsAtPosition(int count, GameObject unit, Vector3 spawnPos)
    {
        for(int i = 0; i < count; i++)
        {
            Instantiate(unit, spawnPos, Quaternion.identity);
        }
    }
}