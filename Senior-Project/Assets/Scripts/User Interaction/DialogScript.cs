using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DialogScript : MonoBehaviour { //Stores a line of dialog for the dialog box
    public string message;
    public Actions endAction;

    private string dialog;
    private int action;
    void Start() {
        dialog = message;
        action = (int)endAction;

    }

    public string getDialog() {
        return dialog;
    }

    public int getAction() { 
        return action; 
    }

    public enum Actions {
        pass, factoryplace, spawnbug, end
    }

}
