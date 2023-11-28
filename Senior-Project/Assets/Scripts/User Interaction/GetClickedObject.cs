using UnityEngine;

public class GetClickedObject : MonoBehaviour
{
    private GameObject troop;
    private GameObject building;
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
                if(hitInfo.transform.CompareTag("Troop"))
                {
                    troop = hitInfo.transform.gameObject;
                }
                else if(hitInfo.transform.CompareTag("Building"))
                {
                    building = hitInfo.transform.gameObject;
                }
            }
            else
            {
                troop = null;
                building = null;
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            troop = null;
            building = null;
        }
    }

    public GameObject getTroop(){return troop;}
    public GameObject getBuilding(){return building;}
}
