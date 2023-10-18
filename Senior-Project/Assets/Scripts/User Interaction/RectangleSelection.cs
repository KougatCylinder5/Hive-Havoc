using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


[RequireComponent(typeof(LineRenderer))]
public class RectangleSelection : MonoBehaviour
{
    public Vector2 _initPos;
    public Vector2 _curPos;
    private Vector3[] _pointList = new Vector3[4];
    private LayerMask _terrainWater;
    private LineRenderer _lineRenderer;
    public UnitController _uc;
    private float height = 0.55f;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _terrainWater = LayerMask.GetMask("Terrain", "Water");
        _uc = GetComponent<UnitController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(detectDrag())
        {
            float hypo = Vector3.Distance(_initPos, _curPos);
            if (hypo > 0.4f)
            {
                _lineRenderer.positionCount = 4;
                float angle = (Mathf.Atan2(_initPos.y - _curPos.y, _initPos.x - _curPos.x) - Mathf.PI / 4);
                float vert = Mathf.Cos(angle) * hypo;
                float horz = Mathf.Sin(angle) * hypo;

                Vector3 startPoint = new Vector3(_initPos.x, height, _initPos.y);
                Vector3 endPoint = new Vector3(_curPos.x, height, _curPos.y);
                Vector3 pointA = new Vector3(_initPos.x + Mathf.Cos(Mathf.PI / -4) * horz, height, _initPos.y + Mathf.Sin(Mathf.PI / -4) * horz);
                Vector3 pointB = new Vector3(_initPos.x + -Mathf.Cos(7 * Mathf.PI / -4) * vert, height, _initPos.y + -Mathf.Sin(7 * Mathf.PI / -4) * vert);

                _pointList[0] = startPoint;
                _pointList[1] = pointA;
                _pointList[2] = endPoint;
                _pointList[3] = pointB;

                _lineRenderer.SetPositions(_pointList);
            }
            else
            {
                _lineRenderer.positionCount = 0;
            }
        }
    }

    public bool detectDrag()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _initPos = Mouse.MouseToWorldPoint(_terrainWater);
            return false;
        }
        if(Input.GetMouseButton(0) && Mouse.MouseToWorldPoint(_terrainWater) != _initPos)
        {
            _curPos = Mouse.MouseToWorldPoint(_terrainWater);
            return true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            selectTroops();
        }
        return false;
    }

    private void selectTroops()
    {
        Vector3 center = (_pointList[0] + _pointList[2]) / 2;
        center.y = 0;
        Vector3 halfExtends = new(Mathf.Abs(_pointList[0].x - _pointList[1].x) / 2, 0f, Mathf.Abs(_pointList[0].z - _pointList[3].z) / 2);


        RaycastHit[] hits = Physics.BoxCastAll(center: center, halfExtents: halfExtends, direction: Vector3.up, Quaternion.Euler(0,45,0), layerMask: LayerMask.GetMask("PlayerUnit"), maxDistance: float.PositiveInfinity);
        foreach(RaycastHit hit in hits)
        {
            _uc.AddUnit(hit.transform.gameObject);
        }
    }
}
