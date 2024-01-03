using System.Collections.Generic;
using UnityEngine;

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
    private const int _resourceCount = 5;
    public Transform boundNearestOrigin;
    public int playableAreaSizeX;
    public int playableAreaSizeZ;

    void Start()
    {
        //Initialize the list of shortcut keys for buildings
        for(int i = 1; i < _buildingPrefabs.Count; i+=2)
        {
            _keycodes.Add(_buildingPrefabs[i].GetComponent<GhostBuildingClass>().GetKey());
        }
    }

    void Update()
    {
        //Check if any of the building shortcut keys are being pressed, and if so, create the building that that key is assigned to.
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
            PlaceBuilding();
        }
    }

    public void DepleteResources(int[] depleteArray)
    {
        ResourceStruct.Wood = depleteArray[0];
        ResourceStruct.Coal = depleteArray[1];
        ResourceStruct.CopperOre = depleteArray[2];
        ResourceStruct.CopperIngot = depleteArray[3];
        ResourceStruct.Stone = depleteArray[4];
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

    public void ShowBuilding(GameObject building, Vector2 position)
    {
        float offset = SizeCheck(building);
        building.transform.position = new Vector3(Mathf.RoundToInt(position.x) + offset, 0.5f, Mathf.RoundToInt(position.y) + offset);
    }

    public float SizeCheck(GameObject building)
    {
        if (building.GetComponent<GhostBuildingClass>().GetSize() % 2 == 0)
        {
            return 0.5f;
        }
        else
        {
            return 0;
        }
    }

    public void ShowGrid(Vector3 nearestOrigin, int width, int length)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4;
        lineRenderer.loop = true;
        lineRenderer.SetPosition(0, nearestOrigin);
        lineRenderer.SetPosition(1, new(nearestOrigin.x, 0.1f, nearestOrigin.z + width));
        lineRenderer.SetPosition(2, new(nearestOrigin.x + length, 0.1f, nearestOrigin.z + width));
        lineRenderer.SetPosition(3, new(nearestOrigin.x + length, 0.1f, nearestOrigin.z));
    }

    public void PlaceBuilding()
    {
        //Show the ghost building and the grid to the player...
        Vector2 position = Mouse.MouseToWorldPoint(LayerMask.GetMask("Terrain", "Water"));
        ShowGrid(boundNearestOrigin.position, playableAreaSizeX, playableAreaSizeZ);
        if (!_made)
        {
            _ghostBuilding = Instantiate(_buildingPrefabs[_pressed * 2 + 1], new Vector3(position.x, 0f, position.y), Quaternion.identity);
            _currentBuilding = _buildingPrefabs[_pressed * 2];
            _made = true;
        }
        ShowBuilding(_ghostBuilding, position);
        //...if the mouse is clicked, try to place the building...
        if (Input.GetMouseButtonDown(0))
        {
            //...if the player doesn't have enough resources, don't let them place it...
            if (CheckCost(_ghostBuilding.GetComponent<GhostBuildingClass>().GetCost(), ResourceStruct.Total, out int[] dep) && _ghostBuilding.GetComponent<GhostBuildingClass>().CheckPlacementArea(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y))/* && CommandCenter.CheckArea(new (Mathf.RoundToInt(position.x), 0, Mathf.RoundToInt(position.y)))*/)
            {
                Saver.allBuildings.Add(Instantiate(_currentBuilding, _ghostBuilding.transform.position, Quaternion.identity));
                DepleteResources(dep);
            }
            else
            {
                _currentBuilding = null;
            }
            _inPlaceMode = false;
            _made = false;
            Destroy(_ghostBuilding);
        }
        //...and if the player presses backspace while a ghost building is active, cancel the placement.
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _currentBuilding = null;
            _inPlaceMode = false;
            _made = false;
            Destroy(_ghostBuilding);
        }
    }

    public void SetGhostBuilding(int ghostBuildingIndex)
    {
        _pressed = ghostBuildingIndex;
        _inPlaceMode = true;
    }
}