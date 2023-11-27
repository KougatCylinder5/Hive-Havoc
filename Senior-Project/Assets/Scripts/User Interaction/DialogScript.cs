using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DialogScript
{
    private string dialog;
    private int action;
    public DialogScript(string text, int nextAction) {
        dialog = text;
        action = nextAction;

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
