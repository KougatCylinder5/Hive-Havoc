using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> units = new();
    public GetClickedObject gco;
    private float r = 0.05f;
    private float n = 0.5f;
    private void Start()
    {
        gco = GetComponent<GetClickedObject>();


    }
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

                Vector2 mousePos = Mouse.MouseToWorldPoint(LayerMask.GetMask("Terrain", "Water"));
                for (int i = 0; i < units.Count; i++)
                {
                    float genX;
                    float genY;
                    do
                    {
                        float j2nr = 2 * j * n / r;
                        genX = r * Mathf.Sqrt(j2nr) * Mathf.Cos(Mathf.Sqrt(j2nr));
                        genY = r * Mathf.Sqrt(j2nr) * Mathf.Sin(Mathf.Sqrt(j2nr));
                        j++;
                    }
                    while (!units[i].GetComponent<UnitAI>().SetDestination(mousePos + new Vector2(genX, genY)));
                }
                Debug.Log(mousePos);
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            units.Clear();
        }
    }
    public bool AddUnit(GameObject unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
            return true;
        }
        return false;
    }
}
