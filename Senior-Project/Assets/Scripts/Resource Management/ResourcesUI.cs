using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    public TMP_Text woodText;
    private int value = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        woodText.text = "Wood: " + ResourceStruct.Wood;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ResourceStruct.Wood += value;
        }
    }
}
