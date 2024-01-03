using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    public TMP_Text woodText;
    public TMP_Text coalText;
    public TMP_Text copperOreText;
    public TMP_Text copperIngotText;
    public TMP_Text stoneText;

    // Set the UI to match the ResourceStruct's values
    void Update()
    {
        woodText.text = "" + ResourceStruct.Wood;
        coalText.text = "" + ResourceStruct.Coal;
        copperOreText.text = "" + ResourceStruct.CopperOre;
        copperIngotText.text = "" + ResourceStruct.CopperIngot;
        stoneText.text = "" + ResourceStruct.Stone;
    }
}
