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
    // Start is called before the first frame update


    // Update is called once per frame


    public void makeASave()
    {
        DBAccess.startTransaction();
        DBAccess.addSave(saveName.text, saveDifficulty.value);
        DBAccess.commitTransaction();
        SceneManager.LoadScene("Tutorial");
    }
}
