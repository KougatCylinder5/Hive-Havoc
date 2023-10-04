using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectTroops : MonoBehaviour
{
    public GetClickedObject getClickedObject;
    public AIController AI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (getClickedObject.getTroop() != null)
            {
                AI = getClickedObject.getTroop().GetComponent<AIController>();
            }
            if (AI != null && getClickedObject.getCube() != null)
            {
                AI.SetDestination(Mouse.MouseToWorldPoint(~LayerMask.GetMask(new string[3]{"Building", "PlayerUnit", "EnemyUnit"})));
            }
        }
            
    }
}
