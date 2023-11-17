using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;

public class SaveManager : MonoBehaviour
{
    private static int pageNumber = 1;
    private static int saveCount = 0;
    private bool x = true;
    public AudioClip pageSound;
    public AudioClip droppedSound;

    void Start()
    {
        refresh();
    }

    public void pageUp()
    {
        if(pageNumber < Mathf.Ceil(saveCount / 4.0f)) {
            pageNumber++;
            try {
                GameObject.FindWithTag("SFX").GetComponent<AudioSource>().clip = pageSound;
                GameObject.FindWithTag("SFX").GetComponent<AudioSource>().time = 0;
                GameObject.FindWithTag("SFX").GetComponent<AudioSource>().Play();
            } catch { }
            
        }
        refresh();
    }

    public void pageDown()
    {   
        if (pageNumber > 1) {
            pageNumber--;
            try {
                GameObject.FindWithTag("SFX").GetComponent<AudioSource>().clip = pageSound;
                GameObject.FindWithTag("SFX").GetComponent<AudioSource>().time = 0;
                GameObject.FindWithTag("SFX").GetComponent<AudioSource>().Play();
            } catch { }
        }
        refresh();
    }

    public void dropped() {
        refresh();

        GameObject.FindWithTag("SFX").GetComponent<AudioSource>().clip = droppedSound;
        GameObject.FindWithTag("SFX").GetComponent<AudioSource>().time = 0;
        GameObject.FindWithTag("SFX").GetComponent<AudioSource>().Play();
    }

    public void refresh()
    {
        DBAccess.startTransaction(false);
        List<Save> saveList = DBAccess.getSaves();
        DBAccess.commitTransaction(false);

        saveCount = saveList.Count;

        PopulateSave[] oldSaves = gameObject.GetComponentsInChildren<PopulateSave>();
        foreach (PopulateSave save in oldSaves) {
            save.show();
        }

        if(x) {
        PopulateSave[] popSaves = gameObject.GetComponentsInChildren<PopulateSave>();
        int topsave = pageNumber * 4;
        foreach(PopulateSave save in popSaves)
        {
            try
            {
                switch (save.slotID)
                {
                    case 1:
                        save.refreshWithSave(saveList[topsave - 4]);
                        break;
                    case 2:
                        save.refreshWithSave(saveList[topsave - 3]);
                        break;
                    case 3:
                        save.refreshWithSave(saveList[topsave - 2]);
                        break;
                    case 4:
                        save.refreshWithSave(saveList[topsave -1]);
                        break;
                }
            }
                catch (Exception)
                {
                save.hide();
            }
        }
            x = true;
        }

    }
}
