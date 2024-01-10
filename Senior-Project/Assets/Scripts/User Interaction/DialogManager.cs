using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;
using static UnityEngine.UI.CanvasScaler;
using System;

public class DialogManager : MonoBehaviour
{
    public List<DialogScript> dialogScript = new List<DialogScript>();
    private int index = -1;
    private int newDialogIndex = 0;
    private int awaitTask = 0;

    private bool triggered = false;

    private void Update() {  //Checks of operation is complete to advance and prints the text.
        if(Saver.LoadDone && !triggered && FlowFieldGenerator.FlowFieldFinished) {
            triggerStart();
        }

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

        if (index > -1 && dialogScript[index].getDialog().Length > newDialogIndex) {
            GetComponentInChildren<TextMeshProUGUI>().text += dialogScript[index].getDialog()[newDialogIndex];
            if (dialogScript[index].getDialog().Length == newDialogIndex) {
            }
            newDialogIndex++;
        }
    }

    public void triggerStart() { //If the box can be enabled.
        int indexStep = 0;
        DBAccess.startTransaction();
        try
        {
            indexStep = DBAccess.getkey("tutorialIndex");
        } catch
        {
            Debug.Log("New Game.");
        }
        DBAccess.commitTransaction();
        if (indexStep == dialogScript.Count-1)
        {
            Destroy(gameObject);
        }
        else if (dialogScript.Count > 0 && !triggered) {
            index = indexStep - 1;
            next();
            triggered = true;
        }
    }

    public void next() { //Advances to next script.
        PauseMenu.setPause(false);
        //Time.timeScale = 0;
        newDialogIndex = 0;
        index++;
        GetComponentInChildren<TextMeshProUGUI>().text = "";
        GetComponent<Canvas>().enabled = true;
    }

    public void nextStep() { //Gets the next step and advances.
        if (dialogScript[index].getAction() == 0) {
            GameObject.Find("Nest(Clone)").GetComponent<SpawnBugs>().enabled = false;
            next();
        } else if (dialogScript[index].getAction() == 1) {
            Time.timeScale = 1;
            GetComponent<Canvas>().enabled = false;
            awaitTask = 1;
            PauseMenu.setPause(true);
        } else if (dialogScript[index].getAction() == 2) {
            Time.timeScale = 1;
            GetComponent<Canvas>().enabled = false;
            GetComponent<SpawnBug>().Spawn();
            awaitTask = 2;
            PauseMenu.setPause(true);
        } else {
            Time.timeScale = 1;
            Destroy(gameObject);
            Saver.allBuildings.FindAll(x => { return x.name[..x.name.IndexOf('(')] == "Nest"; }).ForEach(nest => { nest.GetComponent<SpawnBugs>().enabled = true; });
            PauseMenu.setPause(true);
        }
        DBAccess.startTransaction();
        DBAccess.setKey("tutorialIndex", index);
        DBAccess.commitTransaction();
    }

}
