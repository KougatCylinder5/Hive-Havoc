using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMakerUI : MonoBehaviour
{
    private MakeUnitBuilding mub;

    // Start is called before the first frame update
    void Awake()
    {
        mub = GetComponentInParent<MakeUnitBuilding>();
    }

    public void Spawn()
    {
        mub.CheckForSpawn();
    }
}
