using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPathing : MonoBehaviour
{
    public NpcAI npcai;

    // Start is called before the first frame update
    void Awake()
    {
        npcai.PickUpTarget = GenerateTarget(GetComponentInParent<ResourceCollectBuilding>());
    }

    // Update is called once per frame
    public Vector2 GenerateTarget(ResourceCollectBuilding building)
    {
        Vector3 curTarget = new(10, 1, 10);
        Collider[] colliders = Physics.OverlapSphere(transform.position, building.GetRange(), LayerMask.GetMask("Trees"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if(Vector3.Distance(transform.position, colliders[i].transform.position) < Vector3.Distance(transform.position, curTarget))
            {
                curTarget = colliders[i].transform.position;
            }
        }
        return new(curTarget.x, curTarget.z);
    }
}
