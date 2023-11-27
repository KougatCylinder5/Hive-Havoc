using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    public TMP_Text woodText;
    public TMP_Text coalText;
    public TMP_Text copperOreText;
    public TMP_Text copperIngotText;
    public TMP_Text stoneText;
    private int value = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        woodText.text = "Wood: " + ResourceStruct.Wood;
        coalText.text = "Coal: " + ResourceStruct.Coal;
        copperOreText.text = "CopperOre: " + ResourceStruct.CopperOre;
        copperIngotText.text = "CopperIngot: " + ResourceStruct.CopperIngot;
        stoneText.text = "Stone: " + ResourceStruct.Stone;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ResourceStruct.Wood += value;
            ResourceStruct.Coal += value;
            ResourceStruct.CopperOre += value;
            ResourceStruct.CopperIngot += value;
            ResourceStruct.Stone += value;
        }
    }
}
