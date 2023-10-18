using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildingPrefabs;
    private bool inPlaceMode = false;
    private GameObject currentBuilding;
    private GameObject ghostBuilding;
    private bool _made;
    private List<KeyCode> keycodes = new();
    private int _pressed;

    private enum Buildings
    {
        Base,
        BaseGhost,
        Tent,
        TentGhost,
        Tower,
        TowerGhost
    }

    // Start is called before the first frame update
    void Start()
    {
        keycodes.Add(KeyCode.Z); 
        keycodes.Add(KeyCode.X);
        keycodes.Add(KeyCode.C);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Mouse.MouseToWorldPoint(LayerMask.GetMask("Terrain", "Water"));
        foreach(KeyCode key in keycodes)
        {
            if(Input.GetKeyDown(key))
            {
                inPlaceMode = true;
                _pressed = keycodes.IndexOf(key);
            }
        }
        if(inPlaceMode)
        {
            if(!_made)
            {
                ghostBuilding = Instantiate(buildingPrefabs[_pressed * 2 + 1], new Vector3(position.x, 0.5f, position.y), Quaternion.identity);
                currentBuilding = buildingPrefabs[_pressed * 2];
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
        building.transform.position = new Vector3((int)position.x + sizeCheck(building, 1), 0.5f, (int)position.y + sizeCheck(building, 2));
    }

    public float sizeCheck(GameObject building, int axis)
    {
        if(axis == 1)
        {
            if (building.transform.localScale.x % 2 == 0)
            {
                return 0.5f;
            }
        }
        if(axis == 2)
        {
            if (building.transform.localScale.z % 2 == 0)
            {
                return 0.5f;
            }
        }
        return 0;
    }
}
