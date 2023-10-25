using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildingClass : MonoBehaviour
{
    [SerializeField]
    protected int buildingSizeX = 2;
    [SerializeField]
    protected int buildingSizeY = 2;
    protected int buildingX, buildingY;
    protected int[] buildingCost;

    // Start is called before the first frame update
    void Start()
    {
        buildingX = Mathf.CeilToInt(gameObject.transform.position.x);
        buildingY = Mathf.CeilToInt(gameObject.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int GetBuildingSize()
    {
        return new Vector2Int(buildingSizeX, buildingSizeY);
    }

    public void SetBuildingSize()
    {

    }

    public bool CheckPlacementArea(int x, int y)
    {
        for(int i = 0; i < buildingX * buildingY; i++)
        {
            if (!PathingManager.ObstructedTiles[PathingManager.CalculateIndex(x + buildingX, y + buildingY)])
            {
                return false;
            }
        }
        return true;
    }
}
