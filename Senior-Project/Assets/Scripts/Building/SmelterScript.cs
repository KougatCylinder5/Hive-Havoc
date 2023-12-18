using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterScript : MonoBehaviour
{
    public void ButtonClick()
    {
        ResourceStruct.Coal -= 1;
        ResourceStruct.CopperOre -= 1;
    }

    public void FinishProgress()
    {
        ResourceStruct.CopperIngot += 1;
    }
}
