using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveManager : MonoBehaviour
{
    private static int pageNumber = 1;
    private static int saveCount = 0;
    private bool x = true;

    void Start()
    {
        refresh();
    }

    public void pageUp()
    {
        if(pageNumber < Mathf.Ceil(saveCount / 4.0f)) {
            pageNumber++;
        }
        refresh();
        Debug.Log("Page Number: " + pageNumber);
    }

    public void pageDown()
    {   
        if (pageNumber > 1) {
            pageNumber--;
        }
        refresh();
        Debug.Log("Page Number: " + pageNumber);
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
                        Debug.Log("Save name slot 1: " + saveList[topsave - 4].getSaveName());
                        break;
                    case 2:
                        save.refreshWithSave(saveList[topsave - 3]);
                        Debug.Log("Save name slot 2: " + saveList[topsave - 3].getSaveName());
                        break;
                    case 3:
                        save.refreshWithSave(saveList[topsave - 2]);
                        Debug.Log("Save name slot 3: " + saveList[topsave - 2].getSaveName());
                        break;
                    case 4:
                        save.refreshWithSave(saveList[topsave -1]);
                        Debug.Log("Save name slot 4: " + saveList[topsave - 1].getSaveName());
                        break;
                }
            }
            catch(Exception e)
            {
                save.hide();
                Debug.Log("Problem with: " + save.slotID + " Exc: " + e);
            }
        }
            x = true;
        }

    }
}
