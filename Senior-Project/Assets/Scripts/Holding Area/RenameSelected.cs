using UnityEngine;
using UnityEditor;

public class RenameChildren : EditorWindow
{

    private static readonly Vector2Int _size = new Vector2Int(250, 100);

    private string _childrenPrefix;
    private int _startIndex;
    private string _childrenSuffix;

    [MenuItem("GameObject/Rename Children")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<RenameChildren>();
        window.minSize = _size;
        window.maxSize = _size;
    }

    private void OnGUI()
    {
        _childrenPrefix = EditorGUILayout.TextField("Children prefix", _childrenPrefix);
        _childrenSuffix = EditorGUILayout.TextField("Children suffix", _childrenSuffix);

        _startIndex = EditorGUILayout.IntField("Start index", _startIndex);

        if (GUILayout.Button("Rename children"))
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            for (int objectI = 0; objectI < selectedObjects.Length; objectI++)
            {
                Transform selectedObjectT = selectedObjects[objectI].transform;
                for (int childI = 0, i = _startIndex; childI < selectedObjectT.childCount; childI++) selectedObjectT.GetChild(childI).name = $"{_childrenPrefix}{i++}{_childrenSuffix}";
            }
        }
    }
}