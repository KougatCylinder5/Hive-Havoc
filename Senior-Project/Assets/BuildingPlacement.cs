using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buildingPrefabs;

    private enum Buildings
    {
        Base,
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
        if(Input.GetKeyDown(KeyCode.B))
        {
            Vector2 position = Mouse.MouseToWorldPoint(LayerMask.GetMask("Terrain", "Water"));

            Instantiate(buildingPrefabs[(int)Buildings.Base], new Vector3(position.x, 1, position.y), Quaternion.identity);

        }
    }
}
