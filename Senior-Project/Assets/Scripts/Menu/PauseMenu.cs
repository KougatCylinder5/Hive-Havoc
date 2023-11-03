using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private GUIStyle bgStyle = new GUIStyle();
    public GUIStyle globalBTNStyle;
    private GUIStyle btn1Style = new GUIStyle();
    private GUIStyle btn2Style = new GUIStyle();
    private Texture2D bgImg;
    void Start() {
        bgImg = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
        bgImg.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
        bgImg.Apply();
        bgStyle.normal.background = bgImg;
        bgStyle.normal.textColor = Color.white;
        bgStyle.fontSize = 40;
        bgStyle.alignment = TextAnchor.UpperCenter;
        bgStyle.padding = new RectOffset(0,0,10,0);

        btn1Style = globalBTNStyle;
        btn2Style = globalBTNStyle;

        int buttonOffset = 100;
        btn1Style.margin = new RectOffset(50, 0, buttonOffset, 0);

        buttonOffset += 50;
        btn2Style.margin = new RectOffset(100, 0, buttonOffset, 0);
        

    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Time.timeScale == 0) {
                Time.timeScale = 1;
            } else {
                Time.timeScale = 0;
            }
            
        }
    }

    void OnGUI() {
        if(Time.timeScale == 0) {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "PAUSED", bgStyle);

            if (GUI.Button(new Rect(10, 70, 50, 30), "Resume", btn1Style)) {
                Time.timeScale = 1;
            }
            if (GUI.Button(new Rect(10, 70, 50, 30), "Save And Exit", btn2Style)) {
                DBAccess.startTransaction();
                //Do the save things
                DBAccess.commitTransaction();
                DBAccess.exitSave();
            }
        }
    }
}
