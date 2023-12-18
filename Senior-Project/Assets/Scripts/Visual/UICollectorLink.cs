using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollectorLink : MonoBehaviour
{
    ResourceCollectBuilding building;
    UIPopUps popup;
    void Start()
    {
        building = GetComponentInParent<ResourceCollectBuilding>();
        popup = GetComponent<UIPopUps>();
    }

    // Update is called once per frame
    void Update()
    {
        popup.rate = building.getRate();
    }
}
