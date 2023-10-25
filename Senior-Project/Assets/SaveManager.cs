using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveManager : MonoBehaviour
{
    private static int pageNumber = 1;

    void Start()
    {
        refresh();
    }

    public void pageUp()
    {
        pageNumber++;
        refresh();
    }

    public void pageDown()
    {
        pageNumber--;
        refresh();
    }

    public void refresh()
    {
        DBAccess.startTransaction();
        List<Save> saveList = DBAccess.getSaves();
        DBAccess.commitTransaction();

        Debug.Log(saveList.Count);

        PopulateSave[] popSaves = gameObject.GetComponentsInChildren<PopulateSave>();
        int topsave = pageNumber * 4;
        foreach(PopulateSave save in popSaves)
        {
            try
            {
                save.show();
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
            catch
            {
                save.hide();
            }
        }

    }
}
