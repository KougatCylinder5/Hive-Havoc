using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicScrollview : MonoBehaviour
{
    [SerializeField]
    private Transform scrollViewContent;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private List<int> saveNum;

    private void Start()
    {
        foreach (int icon in saveNum)
        {
            GameObject newIcon = Instantiate(prefab, scrollViewContent);
            //if (newIcon.TryGetComponent<ScrollViewItem>(out ScrollViewItem item));
        }
    }
}
