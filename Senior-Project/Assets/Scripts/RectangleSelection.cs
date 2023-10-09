using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RectangleSelection : MonoBehaviour
{
    private Vector2 _initPos;
    private Vector2 _curPos;
    private Vector3[] _pointList = new Vector3[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(detectDrag())
        {
            _pointList[0] = new Vector3(_initPos.x, 1, _initPos.y);
            _pointList[1] = new Vector3(_curPos.x, 1, _curPos.y);
            //_pointList[2] = new Vector3(_curPos.x, 1, _curPos.y);
            //_pointList[3] = new Vector3(_curPos.x, 1, _initPos.y);
        }
    }

    public bool detectDrag()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _initPos = Mouse.MouseToWorldPoint();
            return false;
        }
        if(Input.GetMouseButton(0) && Mouse.MouseToWorldPoint() != _initPos)
        {
            _curPos = Mouse.MouseToWorldPoint();
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLineList(_pointList);
    }
}
