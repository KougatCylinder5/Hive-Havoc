using UnityEngine;

public class SmelterScript : MonoBehaviour
{
    public void ButtonClick()
    {
        if(ResourceStruct.Coal >= 2 && ResourceStruct.CopperOre >= 5)
        {
            ResourceStruct.Coal -= 2;
            ResourceStruct.CopperOre -= 5;
            FinishProgress();
        }
    }

    public void FinishProgress()
    {
        ResourceStruct.CopperIngot += 1;
    }
}