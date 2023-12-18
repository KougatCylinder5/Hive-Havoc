using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPopUps : MonoBehaviour
{
    public string prefix;
    public UIType uiType;
    public TextMeshProUGUI modifyText;

    public int rate;
    public int max = 0;

    public uint coolDown = 0;
    public EdgeType activateOn; //Only needed if cooldown is greater then 0;

    public UnityEvent Action;

    public UnityEvent OnPress;

    private uint coolDownTime = 0;

    public RectTransform progressBar;

    private bool onGO;

    public enum EdgeType {
        Rising,
        Falling
    }

    public enum UIType {
        Counter,
        Progress
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(uiType == UIType.Counter) {
            modifyText.text = prefix + rate; 
        } else {
            if(coolDownTime > 0) {
                if(coolDownTime == 1 && activateOn == EdgeType.Falling) {
                    Action.Invoke();
                    rate--;
                }
                coolDownTime--;
                progressBar.transform.localScale = new Vector3(coolDownTime * 1.0f / coolDown, 1, 1);
            } else {
                progressBar.transform.localScale = new Vector3(0, 1, 1);
            }
            modifyText.text = prefix + "\n" + rate + "/" + max;
            if (rate > 0 && coolDownTime == 0) {
                coolDownTime = coolDown;

            }
            
        }
    }

    public bool isMouseOver() {
        return onGO;
    }

    public void buttonAction() {
        if(rate < max) {
            rate++;
            OnPress.Invoke();
            if (activateOn == EdgeType.Rising) {
                Action.Invoke();
            }
        }
    }

    public void show() {
        gameObject.transform.localPosition = new Vector3 (0, 3, 0);
    }

    public void hide() {
        gameObject.transform.localPosition = new Vector3(0, -1000, 0);
    }

    private void OnMouseOver() {
        onGO = true;
    }

    private void OnMouseExit() {
        onGO = false;
    }
}