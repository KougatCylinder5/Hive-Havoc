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
    private int awaitTask = 0;

    void Start()
    {
        if (dialogScript.Count > 0) {
            next();
        }
    }

    private void Update() {
        if(awaitTask == 1) {
            if(GameObject.Find("CartMaker(Clone)")) {
                awaitTask = 0;
                next();
            }
        }

        if (awaitTask == 2) {
            if (!GameObject.Find("Crawler(Clone)")) {
                awaitTask = 0;
                next();
            }
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
        GetComponent<Canvas>().enabled = true;
    }

    public void nextStep() {
        if(dialogScript[index].getAction() == 0) {
            next();
        } else if (dialogScript[index].getAction() == 1) {
            GetComponent<Canvas>().enabled = false;
            awaitTask = 1;
        } else if (dialogScript[index].getAction() == 2) {
            GetComponent<Canvas>().enabled = false;
            GetComponent<SpawnBug>().Spawn();
            awaitTask = 2;
        } else {
            Destroy(gameObject);
            GameObject.Find("Nest(").GetComponent<SpawnBugs>().enabled = true;
        }
    }

}
