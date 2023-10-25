using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBugs : MonoBehaviour
{
    public GameObject bug;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnBug), 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBug()
    {
        if (FlowFieldGenerator.Finished)
        {
            Instantiate(bug, transform.position + offset, Quaternion.identity);
        }
        
    }
}
