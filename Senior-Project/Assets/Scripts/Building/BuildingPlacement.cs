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
    private List<GameObject> _buildingPrefabs;
    private bool _inPlaceMode = false;
    private GameObject _currentBuilding;
    private GameObject _ghostBuilding;
    private bool _made;
    private List<KeyCode> _keycodes = new();
    private int _pressed;
    private List<int[]> _costs = new();
    private const int _resourceCount = 8;

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
        _keycodes.Add(KeyCode.Z); 
        _keycodes.Add(KeyCode.X);
        _keycodes.Add(KeyCode.C);
        _keycodes.Add(KeyCode.V);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Mouse.MouseToWorldPoint(LayerMask.GetMask("Terrain", "Water"));
        foreach(KeyCode key in _keycodes)
        {
            if(Input.GetKeyDown(key))
            {
                if(_ghostBuilding != null)
                    Destroy(_ghostBuilding);
                _currentBuilding = null;
                _inPlaceMode = true;
                _pressed = _keycodes.IndexOf(key);
                _made = false;
                break;
            }
        }
        if(_inPlaceMode)
        {
            if(!_made)
            {
                _ghostBuilding = Instantiate(_buildingPrefabs[_pressed * 2 + 1], new Vector3(position.x, 0.5f, position.y), Quaternion.identity);
                _currentBuilding = _buildingPrefabs[_pressed * 2];
                _made = true;
            }
            showBuilding(_ghostBuilding, position);
            if(Input.GetMouseButtonDown(0))
            {
                if (CheckCost(_currentBuilding.GetComponent<BuildingClass>().GetCost(), ResourceStruct.Total, out int[] dep) && _ghostBuilding.GetComponent<BuildingClass>().CheckPlacementArea(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y))) //amognus
                {
                    Instantiate(_currentBuilding, _ghostBuilding.transform.position, Quaternion.identity);
                    DepleteResources(dep);
                }
                else
                {
                    Debug.Log("Not enough resources...");
                    _currentBuilding = null;
                }
                _inPlaceMode = false;
                _made = false;
                Destroy(_ghostBuilding);
            }
        }
    }

    public void DepleteResources(int[] depleteArray)
    {
        ResourceStruct.Wood = depleteArray[0];
        ResourceStruct.Coal = depleteArray[1];
        ResourceStruct.CopperOre = depleteArray[2];
        ResourceStruct.CopperIngot = depleteArray[3];
        ResourceStruct.IronOre = depleteArray[4];
        ResourceStruct.IronIngot = depleteArray[5];
        ResourceStruct.Steel = depleteArray[6];
        ResourceStruct.Stone = depleteArray[7];
    }

    public bool CheckCost(int[] desired, int[] current, out int[] deplete)
    {
        bool flag = true;
        deplete = new int[_resourceCount];
        for(int i = 0; i < _resourceCount; i++)
        {
            if(current[i] < desired[i])
            {
                flag = false;
                break;
            }
            else
            {
                deplete[i] = current[i] - desired[i];
            }
        }
        return flag;
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
