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

    public static Vector2 MouseToWorldPoint(LayerMask mask)
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = _camera.nearClipPlane;
        Ray ray = _camera.ScreenPointToRay(_mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask);
        return new(hit.point.x, hit.point.z);
    }
    public static Vector2 MouseToWorldPoint()
    {
        return MouseToWorldPoint(~0); // LayerMask is what to hit so doing ~0 is hitting all layers
    }

}
