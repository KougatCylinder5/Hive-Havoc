using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DBAccess;

public class LevelSelectManager : MonoBehaviour
{
    UnityEngine.UI.Image[] levelbtns;

    // Start is called before the first frame update
    void Start()
    {
        levelbtns = GetComponentsInChildren<UnityEngine.UI.Image>();

        startTransaction(false);
        if (getPlayedLevels().Contains(1)) {
            levelbtns[1].color = new Color32(107, 255, 161, 255); //Green
            levelbtns[2].color = new Color32(239, 255, 107, 255);
            levelbtns[3].color = new Color32(239, 255, 107, 255);
        } else {
            levelbtns[1].color = new Color32(239, 255, 107, 255); //Yellow
        }

        if(getPlayedLevels().Contains(2)) {
            levelbtns[2].color = new Color32(107, 255, 161, 255);
        }
        if (getPlayedLevels().Contains(3)) {
            levelbtns[3].color = new Color32(107, 255, 161, 255);
        }

        if(getPlayedLevels().Contains(2) && getPlayedLevels().Contains(3)) {
            levelbtns[4].color = new Color32(239, 255, 107, 255);
        }

        if(getPlayedLevels().Contains(4)) {
            levelbtns[4].color = new Color32(107, 255, 161, 255);
            levelbtns[5].color = new Color32(239, 255, 107, 255);
            levelbtns[6].color = new Color32(239, 255, 107, 255);
        }
        if(getPlayedLevels().Contains(5)) {
            levelbtns[5].color = new Color32(107, 255, 161, 255);
            levelbtns[7].color = new Color32(239, 255, 107, 255);
        }
        if (getPlayedLevels().Contains(6)) {
            levelbtns[6].color = new Color32(107, 255, 161, 255);
            levelbtns[8].color = new Color32(239, 255, 107, 255);
        }
        if (getPlayedLevels().Contains(7)) {
            levelbtns[7].color = new Color32(107, 255, 161, 255);
            levelbtns[9].color = new Color32(239, 255, 107, 255);
        }
        if (getPlayedLevels().Contains(8)) {
            levelbtns[8].color = new Color32(107, 255, 161, 255);
        }
        if (getPlayedLevels().Contains(9)) {
            levelbtns[9].color = new Color32(107, 255, 161, 255);
        }
        if(getPlayedLevels().Contains(8) && getPlayedLevels().Contains(9)) {
            levelbtns[10].color = new Color32(239, 255, 107, 255);
        }
        if (getPlayedLevels().Contains(10)) {
            levelbtns[10].color = new Color32(107, 255, 161, 255);
        }

        commitTransaction(false);

    }

    public void play(int level) {
        try {
        startTransaction(false);
        addPlayedLevel(level);
        commitTransaction(false);
        } catch { }
        Start();
    }
}
