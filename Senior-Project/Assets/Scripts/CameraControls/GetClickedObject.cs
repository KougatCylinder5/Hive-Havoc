using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedObject : MonoBehaviour
{
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
                }
                else if(hitInfo.transform.CompareTag("Component"))
                {
                    Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
}
