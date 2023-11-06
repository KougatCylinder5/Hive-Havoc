using UnityEngine;

public class MakeUnits : MonoBehaviour
{
    [SerializeField]
    private GameObject unit;
    [SerializeField]
    private int spawnCount;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            Instantiate(unit, transform.position, transform.rotation);
        }
    }
}