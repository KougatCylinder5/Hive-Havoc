using UnityEngine;

public class PopupDisplayer : MonoBehaviour
{
    public UIPopUps popup;
    private bool goAway = false;
    private bool onGO = false;
    private MakeUnitBuilding mubClass;

    private void Awake()
    {
        mubClass = GetComponent<MakeUnitBuilding>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && mubClass.canMakeUnits) {
            if(onGO) {
                popup.show();
            } else if (!popup.isMouseOver()) {
                popup.hide();
            }
        }
        else
        {
            Debug.Log("Cant make units :(");
        }
    }

    private void OnMouseOver() {
        onGO = true;
    }

    private void OnMouseExit() {
        onGO = false;
    }
}
