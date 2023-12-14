using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBug : MonoBehaviour
{
    public GameObject bug;
    public Vector3 pos;

    public void Spawn()
    {
        if (FlowFieldGenerator.FlowFieldFinished)
        {
            Saver.playerUnits.Add(Instantiate(bug, pos, Quaternion.identity));
        }
        
    }
}
