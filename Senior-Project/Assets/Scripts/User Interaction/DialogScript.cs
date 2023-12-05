using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DialogScript : MonoBehaviour {
    public string message;
    public int endAction;

    private string dialog;
    private int action;
    void Start() {
        dialog = message;
        action = endAction;

    }

    public string getDialog() {
        return dialog;
    }

    public int getAction() { 
        return action; 
    }

    public enum actions {
        pass, wait, end
    }

}
