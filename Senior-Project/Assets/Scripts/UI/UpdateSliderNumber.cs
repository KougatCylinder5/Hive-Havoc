using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateSliderNumber : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI number;

    private void Update()
    {
        number.text = slider.value.ToString();
    }
}
