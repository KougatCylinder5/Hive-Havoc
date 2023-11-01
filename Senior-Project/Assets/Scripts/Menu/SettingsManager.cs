using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

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
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + float.Parse(resolutions[i].refreshRateRatio.ToString()) + "hz";
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
}
