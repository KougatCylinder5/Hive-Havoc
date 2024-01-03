using System.Collections.Generic;
using UnityEngine;

public class WinLoseCondition : MonoBehaviour
{
    public static GameObject commandCenter;
    public static List<GameObject> nests = new();
    public GameObject winScreen;
    public GameObject loseScreen;

    //Check for winning or losing every second
    void Awake()
    {
        InvokeRepeating(nameof(CheckWin), 10, 1);
        InvokeRepeating(nameof(CheckLose), 10, 1);
    }

    void CheckWin()
    {
        //If there are no more nests to destroy, the player wins.
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
    //If the command center has been destroyed, the player loses.
    void CheckLose()
    {
        loseScreen.SetActive(commandCenter == null);  
    }
}