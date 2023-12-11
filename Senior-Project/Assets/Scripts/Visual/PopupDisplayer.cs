using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDisplayer : MonoBehaviour
{
    public UIPopUps popup;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            popup.show();
        } else {
            popup.hide();
        }
    }
}
