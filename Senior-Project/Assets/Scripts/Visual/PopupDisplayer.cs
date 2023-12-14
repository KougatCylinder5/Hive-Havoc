using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDisplayer : MonoBehaviour
{
    public UIPopUps popup;
    private bool goAway = false;
    private bool onGO = false;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if(onGO) {
                popup.show();
            } else if (!popup.isMouseOver()) {
                popup.hide();
            }
        }
    }

    private void OnMouseOver() {
        onGO = true;
    }

    private void OnMouseExit() {
        onGO = false;
    }
}
