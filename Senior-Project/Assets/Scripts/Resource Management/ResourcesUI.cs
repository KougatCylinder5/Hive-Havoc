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

    // Update is called once per frame
    void Update()
    {
        woodText.text = "" + ResourceStruct.Wood;
        coalText.text = "" + ResourceStruct.Coal;
        copperOreText.text = "" + ResourceStruct.CopperOre;
        copperIngotText.text = "" + ResourceStruct.CopperIngot;
        stoneText.text = "" + ResourceStruct.Stone;
    }
}
