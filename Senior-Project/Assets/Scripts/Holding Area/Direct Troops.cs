using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectTroops : MonoBehaviour
{
    public GetClickedObject gco;
    public AIController AI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gco.getTroop() != null)
        {
            AI = gco.getTroop().GetComponent<AIController>();
            if (getClickedObject.getTroop() != null)
            {
                AI = getClickedObject.getTroop().GetComponent<AIController>();
            }
            if (AI != null && getClickedObject.getCube() != null)
            {
                AI.SetDestination(Mouse.MouseToWorldPoint(~LayerMask.GetMask(new string[4]{"Terrain", "Building", "PlayerUnit", "EnemyUnit"})));
            }
        }
        if(gco.getTroop() != null && gco.getCube() != null && Input.GetMouseButton(0))
        {
            AI.SetDestination(Mouse.MouseToWorldPoint(~LayerMask.GetMask(new string[3]{"Building", "PlayerUnit", "EnemyUnit"})));
        }   
    }
}
