using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider SensSlider;


    public void LoadAll()
    {
        LoadSens();
    }
    public void SensChange(float Sens)
    {
        PlayerPrefs.SetFloat("Sensitivity", Sens);
    }
    public void LoadSens()
    {
        SensSlider.value = PlayerPrefs.GetFloat("Sensitivity");
    }
    public void Save()
    {
        PlayerPrefs.Save();
    }
}
