using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedObject : MonoBehaviour
{
    public GameObject troop;
    public GameObject cube;
    public LayerMask notTerrain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(ray:Camera.main.ScreenPointToRay(Input.mousePosition),maxDistance: float.MaxValue, hitInfo: out hitInfo,layerMask: notTerrain);
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