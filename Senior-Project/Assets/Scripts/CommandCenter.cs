using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    private static Vector3 pos;
    void Awake()
    {
        pos = transform.position;
    }
    public static bool CheckArea(Vector3 position)
    {
        if(Vector3.Distance(pos, position) < 20)
        {
            return true;
        }
        return false;
    }
}
