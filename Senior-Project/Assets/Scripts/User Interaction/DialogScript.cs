using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DialogScript : MonoBehaviour {
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
        pass, factoryplace, wait, end
    }

}
