using UnityEngine;
using static PathingManager;

public class GhostBuildingClass : MonoBehaviour
{
    [SerializeField]
    protected int buildingSize;
    [SerializeField]
    protected int[] buildingCost;
    [SerializeField]
    protected KeyCode key;
    protected GameBounds bounds;

    private void Awake()
    {
        bounds = GameObject.Find("ScriptManager").GetComponent<GameBounds>();
    }

    public bool CheckPlacementArea(int x, int y)
    {
        x += Mathf.FloorToInt(buildingSize / 2f);
        y += Mathf.FloorToInt(buildingSize / 2f);
        for (int i = 0; i < buildingSize * buildingSize; i++)
        {
            Vector2 testPos = new(x - i % buildingSize, y - i / buildingSize);

            if (!IsOpen(testPos) || !bounds.IsInBounds(testPos))
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