using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public List<GameObject> nests;

    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating(nameof(CheckWin), 0, 1);
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
            SceneManager.LoadScene("LevelSelect");
        }
            
    }
}
