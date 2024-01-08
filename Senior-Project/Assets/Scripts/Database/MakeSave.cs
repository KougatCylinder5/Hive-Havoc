using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MakeSave : MonoBehaviour
{
    public TMP_InputField saveName;
    public TMP_Dropdown saveDifficulty;


    public void makeASave() // Create a save.
    {
        DBAccess.startTransaction();
        DBAccess.addSave(saveName.text, saveDifficulty.value);
        DBAccess.commitTransaction();
        DBAccess.startTransaction();
        DBAccess.selectSave(saveName.text);
        DBAccess.commitTransaction();
    }
}
