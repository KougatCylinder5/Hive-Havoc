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
        if (dialogScript.Count > 0) {
            next();
        }
    }

    private void FixedUpdate() {
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
