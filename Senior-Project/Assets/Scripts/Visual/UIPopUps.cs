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


    private uint coolDownTime = 0;


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
            }
            modifyText.text = prefix + "\n" + rate + "/" + max;
        }

    }

    public void buttonAction() {
        if(rate < max) {
            if(rate == 0) {
                coolDownTime = coolDown;
            }
            rate++;
        }
    }
}