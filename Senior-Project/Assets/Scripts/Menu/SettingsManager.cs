using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using static System.Net.Mime.MediaTypeNames;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public static KeyCode key1 = KeyCode.Alpha1;
    public static KeyCode key2 = KeyCode.Alpha2;
    public static KeyCode key3 = KeyCode.Alpha3;
    public static KeyCode key4 = KeyCode.Alpha4;
    public static KeyCode key5 = KeyCode.Alpha5;
    public static KeyCode key6 = KeyCode.Alpha6;
    public static KeyCode key7 = KeyCode.Alpha7;
    public static KeyCode key8 = KeyCode.Alpha8;
    public static KeyCode key9 = KeyCode.Alpha9;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        //variable for setting the resolution to users defult screen size
        int currentResolutionIndex = 0;

        //gets all of the possible screen resolutions and formats them
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + decimal.Round(decimal.Parse(resolutions[i].refreshRateRatio.ToString()), 2) + "hz";
            options.Add(option);

            //sees if the screen size is the users screen size
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        //adds the options to the dropdown
        resolutionDropdown.AddOptions(options);
        //sets the resolution to users defult screen size
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        printKeys();
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void printKeys()
    {
        //Looks at each key that could possibly be pressed
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            //if the key it is looking at is down, print it
            if (Input.GetKeyDown(kcode))
            {
                Debug.Log(kcode);
            }
        }
    }
}
