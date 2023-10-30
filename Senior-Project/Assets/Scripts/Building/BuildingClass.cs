using UnityEngine;
using static PathingManager;

public class BuildingClass : MonoBehaviour
{
    [SerializeField]
    protected int buildingSize;
    [SerializeField]
    protected int[] buildingCost;
    protected int health, maxHealth;
    [SerializeField]
    protected KeyCode key;

    public bool CheckPlacementArea(int x, int y)
    {
        x += Mathf.FloorToInt(buildingSize / 2f);
        y += Mathf.FloorToInt(buildingSize / 2f);
        for(int i = 0; i < buildingSize * buildingSize; i++)
        {
            if (!IsOpen(new(x - i % buildingSize, y - i / buildingSize)))
            {
                return false;
            }
        }
        return true;
    }

    public int[] GetCost()
    {
        return buildingCost;
    }

    public KeyCode GetKey()
    {
        return key;
    }

    public int GetSize()
    {
        return buildingSize;
    }
}
