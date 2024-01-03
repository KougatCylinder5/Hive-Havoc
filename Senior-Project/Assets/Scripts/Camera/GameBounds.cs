using UnityEngine;

public class GameBounds : MonoBehaviour
{
    public GameObject bound1;
    public GameObject bound2;
    public GameObject bound3;
    public GameObject bound4;
    private Rect rect;
    public int terrainSizeX;
    public int terrainSizeY;

    private void Awake()
    {
        rect = MakeRect(bound3, bound1);
    }

    public Rect MakeRect(GameObject nearestToOrigin, GameObject furthestFromOrigin)
    {
        return new()
        {
            x = nearestToOrigin.transform.position.x,
            y = nearestToOrigin.transform.position.z,
            width = furthestFromOrigin.transform.position.x + 1 - nearestToOrigin.transform.position.x,
            height = furthestFromOrigin.transform.position.z + 1 - nearestToOrigin.transform.position.z
        };
    }
    //The camera must stay within the bounds.
    public Vector3 StayInBounds(Vector3 position)
    {
        return new Vector3(Mathf.Clamp(position.x, bound4.transform.position.x, bound2.transform.position.x), position.y, Mathf.Clamp(position.z, bound3.transform.position.z, bound1.transform.position.z));
    }

    public bool IsInBounds(Vector3 position)
    {
        return rect.Contains(position);
    }

    public Vector2Int GetTerrainSize()
    {
        return new(terrainSizeX, terrainSizeY);
    }
}