using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterScript : MonoBehaviour
{
    ResourceConvertBuilding rcb;


    private void Awake()
    {
        rcb = GetComponentInParent<ResourceConvertBuilding>();
    }
    public void ButtonClick()
    {
        rcb.ConvertResources();
    }
}
