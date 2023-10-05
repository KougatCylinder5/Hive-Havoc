using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public List<GameObject> units = new();
    public GetClickedObject gco;
    private float r = 0.15f;
    private float n = 0.5f;

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
                int j = 0;
                for(int i = 0; i < units.Count; i++)
                {
                    float genX;
                    float genY;
                    do
                    {
                        float idk = 2 * j * n / r;
                        genX = r * Mathf.Sqrt(idk) * Mathf.Cos(Mathf.Sqrt(idk));
                        genY = r * Mathf.Sqrt(idk) * Mathf.Sin(Mathf.Sqrt(idk));
                        j++;
                    }
                    while (!units[i].GetComponent<AIController>().SetDestination(Mouse.MouseToWorldPoint() + new Vector2(genX, genY)));
                    
                    
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            units.Clear();
        }
    }
}
