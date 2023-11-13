using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private GUIStyle bgStyle = new GUIStyle();
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

    public void OnGUI() {
        if(Time.timeScale == 0) {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "PAUSED", bgStyle);

            GUILayout.BeginArea(new Rect(Screen.width/3, 0, Screen.width/3, Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resume")) {
                Time.timeScale = 1;
            }
            if (GUILayout.Button("Save and Quit")) {
                GameObject.Find("Saver").GetComponent<Saver>().saveScene();
                Time.timeScale = 1;
            }
            if (GUILayout.Button("FIX IT!")) {
                GameObject.Find("Saver").GetComponent<Saver>().quickFix();
                Time.timeScale = 1;
            }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }
    }
}
