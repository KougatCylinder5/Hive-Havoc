using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateSave : MonoBehaviour
{
    // Start is called before the first frame update
    public int slotID;
    private TextMeshProUGUI[] textFeilds;
    void Start()
    {
        //refreshWithSave(new Save("Test", 1, 0, "0", 1, "NONE", "Home"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hide() {
        gameObject.SetActive(false);
    }

    public void show() {
        gameObject.SetActive(true);
    }

    public void refreshWithSave(Save saveData) {
        textFeilds = GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI textFeild in textFeilds) {
            textFeild.text = textFeild.text.Replace("{name}", saveData.getSaveName());
            textFeild.text = textFeild.text.Replace("{current_level}", saveData.getLevelName());
            textFeild.text = textFeild.text.Replace("{playtime}", "N/a");
            if(textFeild.text.Contains("{difficulty}")) {
                switch(saveData.getDif()) {
                    case (0):
                        textFeild.color = new Color(112 / 255.0f, 226 / 255.0f, 112 / 255.0f);
                        textFeild.text = textFeild.text.Replace("{difficulty}", "Easy");
                        break;
                    case (1):
                        textFeild.color = new Color(239 / 255.0f, 243 / 255.0f, 70 / 255.0f);
                        textFeild.text = textFeild.text.Replace("{difficulty}", "Normal");
                        break;
                    case (2):
                        textFeild.color = new Color(217 / 255.0f, 85 / 255.0f, 77 / 255.0f);
                        textFeild.text = textFeild.text.Replace("{difficulty}", "Hard");
                        break;
                    case (3):
                        textFeild.color = new Color(147 / 255.0f, 59 / 255.0f, 53 / 255.0f);
                        textFeild.text = textFeild.text.Replace("{difficulty}", "Expert");
                        break;
                    default:
                        textFeild.text = textFeild.text.Replace("{difficulty}", "None");
                        break;
                }
                
            }
            
            
        }
    }
}
