using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public List<GameObject> units = new();
    public GetClickedObject gco;
    private float r = 0.05f;
    private float n = 0.5f;


    private void Start()
    {
        gco = GetComponent<GetClickedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                GameObject temp = gco.getTroop();
                if (!units.Contains(temp))
                {
                    units.Add(temp);
                }
            }
            else
            {
                int j = 1;

                Vector2 mousePos = Mouse.MouseToWorldPoint();
                for (int i = 0; i < units.Count; i++)
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
                    while (!units[i].GetComponent<AIController>().SetDestination(mousePos + new Vector2(genX, genY)));
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            units.Clear();
        }
    }
}
