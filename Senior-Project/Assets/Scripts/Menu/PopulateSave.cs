using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class PopulateSave : MonoBehaviour
{
    public int slotID;
    private TextMeshProUGUI[] textFields;
    private string saveName = "";

    public GameObject savePrefab;
    void Start()
    {
 
    }

    public void dropSave() { //Deletes a save.
        DBAccess.startTransaction();
        DBAccess.deleteSave(saveName);
        DBAccess.commitTransaction();
        
        GetComponentInParent<SaveManager>().dropped();
    }

    public void loadSave() { //Loads in a save as a reloaded save.
        DBAccess.startTransaction();
        DBAccess.doReload();
        DBAccess.selectSave(saveName);
        DBAccess.commitTransaction();
    }

    public string getSaveName() {
        return saveName;
    }

    public void setSlot(int newSlotID) {
        slotID = newSlotID;
    }

    public void hide() {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    public void show() { //Creates a save frame with the save information.
        
        GameObject newFrame = UnityEngine.Object.Instantiate(savePrefab);
        newFrame.transform.SetParent(GameObject.Find("Profile Menu").transform);
        newFrame.transform.position = gameObject.transform.position;
        newFrame.transform.localScale = new Vector3(1,1,1);
        newFrame.GetComponent<PopulateSave>().setSlot(slotID);
        newFrame.GetComponent <PopulateSave>().savePrefab = Resources.Load("Save") as GameObject;
        newFrame.name = "Save";
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void refreshWithSave(Save saveData) { //Reloads the saves
        textFields = GetComponentsInChildren<TextMeshProUGUI>();

        saveName = saveData.getSaveName();

        foreach (TextMeshProUGUI textFeild in textFields) {
            textFeild.text = textFeild.text.Replace("{name}", saveData.getSaveName());
            textFeild.text = textFeild.text.Replace("{current_level}", saveData.getLevelName());
            textFeild.text = textFeild.text.Replace("{playtime}", saveData.getPlayTime() + "");
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
