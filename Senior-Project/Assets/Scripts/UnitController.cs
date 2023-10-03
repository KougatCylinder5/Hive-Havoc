using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public List<GameObject> units = new();
    public GetClickedObject gco;
    private int _unitSpacing = 4;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Input.GetKey(KeyCode.LeftControl))
            {
                GameObject temp = gco.getTroop();
                if(!units.Contains(temp))
                {
                    units.Add(temp);
                }
            }
            else
            {
                foreach (GameObject unit in units)
                {
                    unit.GetComponent<AIController>().SetDestination(Mouse.MouseToWorldPoint() + (Random.insideUnitCircle * units.Count / _unitSpacing));
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            units.Clear();
        }
    }
}
