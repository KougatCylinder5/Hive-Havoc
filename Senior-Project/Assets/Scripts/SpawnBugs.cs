using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBugs : MonoBehaviour
{
    public GameObject bug;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBug", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBug()
    {
        Instantiate(bug, transform.position, Quaternion.identity);
    }
}