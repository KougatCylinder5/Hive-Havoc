using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseCondition : MonoBehaviour
{
    public List<GameObject> nests;
    public GameObject commandCenter;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating(nameof(CheckWin), 0, 1);
        InvokeRepeating(nameof(CheckLose), 0, 1);
    }

    void CheckWin()
    {
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
            Debug.Log("Win!");
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
            Debug.Log("Lose...");
        }
    }
}
