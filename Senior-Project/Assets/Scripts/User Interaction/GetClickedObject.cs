using UnityEngine;

public class GetClickedObject : MonoBehaviour
{
    private GameObject troop;
    private GameObject building;
    private GameObject popup;
    public LayerMask notTerrain;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(ray:Camera.main.ScreenPointToRay(Input.mousePosition),maxDistance: float.MaxValue, hitInfo: out hitInfo,layerMask: notTerrain);
            if(hit)
            {
                if(hitInfo.transform.CompareTag("Troop"))
                {
                    troop = hitInfo.transform.gameObject;
                }
                else if(hitInfo.transform.CompareTag("Building"))
                {
                    building = hitInfo.transform.gameObject;
                }
                else if(hitInfo.transform.CompareTag("UI"))
                {
                    popup = hitInfo.transform.gameObject;
                }
            }
            else
            {
                troop = null;
                building = null;
                popup = null;
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            troop = null;
            building = null;
            popup = null;
        }
    }

    public GameObject getTroop(){return troop;}
    public GameObject getBuilding(){return building;}
    public GameObject getPopup(){return popup;}
}