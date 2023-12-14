using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDisplayer : MonoBehaviour
{
    public UIPopUps popup;
    public bool goAway = false;


    private void Update() {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (!goAway)
            {
                popup.show();
                goAway = true;
            }
            else if (goAway)
            {
                popup.hide();
                goAway = false;
            }
        }
    }
}
