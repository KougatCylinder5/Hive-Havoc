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
    private List<Sprite> icons;

    private void Start()
    {
        foreach (Sprite icon in icons)
        {
            GameObject newIcon = Instantiate(prefab, scrollViewContent);
            if(newIcon.TryGetComponent<ScrollViewItem>(out ScrollViewItem item)){
                item.ChangeImage(icon);
            }
        }
    }
}
