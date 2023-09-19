using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{

    private static Camera _camera;
    private static Vector3 _mousePosition = Vector3.zero;

    private void Start()
    {
        _camera = Camera.main;
    }

    public static Vector2 MouseToWorldPoint()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = _camera.nearClipPlane;
        Ray ray = _camera.ScreenPointToRay(_mousePosition);
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.point;
    }

}
