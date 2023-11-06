using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedObject : MonoBehaviour
{
    public GameObject troop;
    public GameObject cube;
    public LayerMask playerUnits;

    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            bool hit = Physics.Raycast(ray:Camera.main.ScreenPointToRay(Input.mousePosition),maxDistance: float.MaxValue, hitInfo: out RaycastHit hitInfo,layerMask: playerUnits);
            if(hit)
            {
                if(hitInfo.transform.CompareTag("Cube"))
                {
                    cube = hitInfo.transform.gameObject;
                }
                else if(hitInfo.transform.CompareTag("Troop"))
                {
                    troop = hitInfo.transform.gameObject;
                }
            }
            else
            {
                troop = null;
                cube = null;
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            troop = null;
            cube = null;
        }
    }

    public GameObject getTroop(){return troop;}
    public GameObject getCube(){return cube;}
    public void resetCube(){cube = null;}
}
