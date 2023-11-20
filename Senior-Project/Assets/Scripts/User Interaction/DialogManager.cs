using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{

    public List<DialogScript> dialogScript = new List<DialogScript>();
    private int index = -1;
    private int newDialogIndex = 0;

    void Start()
    {
        dialogScript.Add(new DialogScript("This is page 1", 0));
        dialogScript.Add(new DialogScript("This is page 2", 0));
        dialogScript.Add(new DialogScript("This is page 3", 0));
        dialogScript.Add(new DialogScript("This is page 4", 0));
        dialogScript.Add(new DialogScript("This is the last page", 2));
        if (dialogScript.Count > 0) {
            next();
        }
    }

    private void Update() {
        if(dialogScript[index].getDialog().Length > newDialogIndex) {
            GetComponentInChildren<TextMeshProUGUI>().text += dialogScript[index].getDialog()[newDialogIndex];
            if(dialogScript[index].getDialog().Length == newDialogIndex) {
            }
            newDialogIndex++;
        }
    }

    public void next() {
        newDialogIndex = 0;
        index++;
        GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    public void nextStep() {
        if(dialogScript[index].getAction() == 0) {
            next();
        } else {
            Destroy(gameObject);
        }
    }

}
