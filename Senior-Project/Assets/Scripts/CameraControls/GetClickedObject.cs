using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedObject : MonoBehaviour
{
    public GameObject troop;
    public GameObject cube;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if(hit)
            {
                if(hitInfo.transform.CompareTag("Cube"))
                {
                    Debug.Log("Hit " + hitInfo.transform.gameObject.name + " in " + hitInfo.transform.parent.name + " in " + hitInfo.transform.parent.parent.name);
                    cube = hitInfo.transform.gameObject;
                }
                else if(hitInfo.transform.CompareTag("Troop"))
                {
                    Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                    troop = hitInfo.transform.gameObject;
                }
            }
            else
            {
                Debug.Log("No hit");
                troop = null;
                cube = null;
            }
        }
    }

    public GameObject getTroop(){return troop;}
    public GameObject getCube(){return cube;}
}
