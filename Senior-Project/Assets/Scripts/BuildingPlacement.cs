using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildingPrefabs;
    public bool inPlaceMode = false;
    public GameObject currentBuilding;
    public GameObject ghostBuilding;
    private bool _made;

    private enum Buildings
    {
        Base,
        BaseGhost,
        Tent,
        Wood_House,
        Stone_House
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Mouse.MouseToWorldPoint(LayerMask.GetMask("Terrain", "Water"));

        if (Input.GetKeyDown(KeyCode.B))
        {
            inPlaceMode = true;
        }
        if(inPlaceMode)
        {
            if(!_made)
            {
                ghostBuilding = Instantiate(buildingPrefabs[(int)Buildings.BaseGhost], new Vector3(position.x, 1, position.y), Quaternion.identity);
                currentBuilding = buildingPrefabs[(int)Buildings.Base];
                _made = true;
            }
            showBuilding(ghostBuilding, position);
            if(Input.GetMouseButtonDown(0))
            {
                inPlaceMode = false;
                _made = false;
                Instantiate(currentBuilding, ghostBuilding.transform.position, Quaternion.identity);
                Destroy(ghostBuilding);
            }
        }
    }

    public void showBuilding(GameObject building, Vector2 position)
    {
        building.transform.position = new Vector3((int)position.x + 0.5f, 1, (int)position.y + 0.5f);
    }
}
