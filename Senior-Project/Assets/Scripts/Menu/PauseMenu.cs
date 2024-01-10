using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private GUIStyle bgStyle = new GUIStyle();
    private Texture2D bgImg;
    private GUIStyle bgStyle2 = new GUIStyle();
    private Texture2D bgImg2;
    private string dots = "";
    private int dotsCounter = 0;

    private static bool canPause = true;
    void Start() { //Creates the pause menu style.
        bgImg = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
        bgImg.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
        bgImg.Apply();
        bgStyle.normal.background = bgImg;
        bgStyle.normal.textColor = Color.white;
        bgStyle.fontSize = 40;
        bgStyle.alignment = TextAnchor.UpperCenter;
        bgStyle.padding = new RectOffset(0,0,10,0);

        bgImg2 = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);
        bgImg2.SetPixel(0, 0, new Color(0, 0, 0, 0.8f));
        bgImg2.Apply();
        bgStyle2.normal.background = bgImg2;
        bgStyle2.normal.textColor = Color.white;
        bgStyle2.fontSize = 40;
        bgStyle2.alignment = TextAnchor.MiddleCenter;
        bgStyle2.padding = new RectOffset(0, 0, 10, 0);
    }

    public static void setPause(bool status) { //Sets if the game is allowed to pause.
        canPause = status;
    }

    void Update() { //Updates the pause state if allowed and escape is pressed.
        if(Input.GetKeyDown(KeyCode.Escape) && canPause) {
            if(Time.timeScale == 0) {
                Time.timeScale = 1;
            } else {
                Time.timeScale = 0;
            }
        }
    }

    public void OnGUI() { //Puts the elements onto the screen.
        if(Time.timeScale == 0 && canPause) {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "PAUSED", bgStyle);

            GUILayout.BeginArea(new Rect(Screen.width/3, 0, Screen.width/3, Screen.height));
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Resume")) {
                Time.timeScale = 1;
            }
            if (GUILayout.Button("Save and Quit")) {
                Application.Quit();
            }

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }

        if (!Saver.LoadDone || !FlowFieldGenerator.FlowFieldFinished) {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Loading" + dots, bgStyle2);

            if(!(dotsCounter > 0)) {
                dotsCounter = 30;
                dots += ".";
                if(dots == "....") {
                    dots = "";
                }
            }
            dotsCounter--;
        } else if(bgImg2.GetPixel(0,0).a > 0) {
            bgImg2.SetPixel(0, 0, new Color(0, 0, 0, bgImg2.GetPixel(0, 0).a - 0.005f));
            bgImg2.Apply();
            bgStyle2.normal.background = bgImg2;
            bgStyle2.normal.textColor = new Color(1, 1, 1, bgImg2.GetPixel(0, 0).a + 0.2f);
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Ready?", bgStyle2);
        }
    }
}
