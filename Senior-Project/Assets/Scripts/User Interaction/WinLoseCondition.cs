using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseCondition : MonoBehaviour
{
    public GameObject commandCenter;
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
        if (!Saver.LoadDone)
            return;
        bool hasWon = false;
        foreach (GameObject go in nests)
        {
            if (go != null)
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
        bool hasLost = false;
        if (commandCenter != null)
        {
            hasLost = true;
        }
        if(hasLost)
        {
            loseScreen.SetActive(true);
        }
    }
}
