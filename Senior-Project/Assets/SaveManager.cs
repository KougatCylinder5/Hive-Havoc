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
                        Debug.Log(saveList[topsave - 4].getSaveName());
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
            catch(Exception e)
            {
                save.hide();
                Debug.Log("Problem with: " + save.slotID + " Exc: " + e);
            }
        }

    }
}
