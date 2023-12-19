using System.Collections.Generic;
using UnityEngine;

public class WinLoseCondition : MonoBehaviour
{
    public static GameObject commandCenter;
    public static List<GameObject> nests = new();
    public GameObject winScreen;
    public GameObject loseScreen;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating(nameof(CheckWin), 10, 1);
        InvokeRepeating(nameof(CheckLose), 10, 1);
    }

    void CheckWin()
    {
        bool hasWon = false;
        foreach (GameObject nests in nests)
        {
            if (nests != null)
            {
                hasWon = false;
                break;
            }
            hasWon = true;
        }
        if (hasWon)
        {
            winScreen.SetActive(true);
        }
            
    }

    void CheckLose()
    {
        print(commandCenter);
        if (commandCenter == null)
        {
            loseScreen.SetActive(true);  
        }
    }
}
