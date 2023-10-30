using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{

    public Toggle fullscreenTog, VsyncTog;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;
    public TMP_Text resolutionLable;
    // Start is called before the first frame update
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            VsyncTog.isOn = false;
        }
        else
        {
            VsyncTog.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLable();
    }

    public void ResRight() 
    {
        selectedResolution++;
        if(selectedResolution > resolutions.Count - 1)
        { 
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLable();
    }

    public void UpdateResLable()
    {
        resolutionLable.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphicsChange()
    {
        //Screen.fullScreen = fullscreenTog.isOn;
        if (VsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
