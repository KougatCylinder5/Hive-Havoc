using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float minVal = 0;
    public float maxVal = 100;
    public float amount = 50;
    public Image progressBar;
    public TextMeshProUGUI amountText;

    public void Update()
    {
        barAmount();
        changeColor();
        amount = Mathf.Clamp(amount, minVal,maxVal);
    }

    public void barAmount()
    {
        progressBar.fillAmount = amount/maxVal;
        amountText.SetText(amount.ToString());
    }

    public void changeColor()
    {
        if (amount <= 50)
        {
            progressBar.color = Color.green;
        }
        else if(amount < 100)
        {
            progressBar.color = Color.yellow;
        }
        else if(amount == 100)
        {
            progressBar.color = Color.red;
        }
            
    }
}
