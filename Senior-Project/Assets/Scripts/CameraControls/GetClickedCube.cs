using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedCube : MonoBehaviour
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
            if(hit && hitInfo.transform.CompareTag("Cube"))
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name + " in " + hitInfo.transform.parent.name + " in " + hitInfo.transform.parent.parent.name);
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
}
