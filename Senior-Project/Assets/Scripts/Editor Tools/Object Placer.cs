using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


[RequireComponent(typeof(LineRenderer))]
public class ObjectPlacer : MonoBehaviour
{
    public Vector2 _initPos;
    public Vector2 _curPos;
    private Vector3[] _pointList = new Vector3[4];
    private LayerMask _terrainWater;
    private Terrain _terrain;
    private List<TreeInstance> treeData;
    private LineRenderer _lineRenderer;
    private float height = 0.55f;

    [SerializeField]
    private ItemsID type;
    

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _terrainWater = LayerMask.GetMask("Terrain", "Water");
        _terrain = GameObject.Find("Ground").GetComponent<Terrain>();
        _lineRenderer.loop = true;
        _lineRenderer.widthMultiplier = 0.1f;
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
                Vector3 pointA = new Vector3(_curPos.x, height, _initPos.y);
                Vector3 pointB = new Vector3(_initPos.x, height, _curPos.y);

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
        Debug.Log(Input.GetMouseButtonUp(0));
        if (Input.GetMouseButtonUp(0))
        {
            placeObjects();
            _lineRenderer.positionCount = 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AssetDatabase.CreateAsset(_terrain.terrainData, "Assets/ExportedTerrainData.asset");
            AssetDatabase.SaveAssets();
        }
        return false;
    }

    private void placeObjects()
    {
        Debug.Log((int)Math.Round(_pointList[0].x) + ", X" + (int)Math.Round(_pointList[0].x));
        Debug.Log((int)Math.Round(_pointList[0].z) + ", Y" + (int)Math.Round(_pointList[2].z));
        for (int x = (int)Math.Round(_pointList[0].x); x < _pointList[2].x; x++)
        {
            Debug.Log(x);
            for (int z = (int)Math.Round(_pointList[0].z); z < _pointList[2].z; z++)
            {
          
                TreeInstance tree = new();
                tree.prototypeIndex = (int)type;
                tree.position = new Vector3((x + 0.5f) / _terrain.terrainData.size.x, 0, (z + 0.5f) / _terrain.terrainData.size.z);
                tree.widthScale = 1;
                tree.heightScale = 1;
                tree.rotation = 0;
                Debug.Log(_terrain.terrainData.treeInstanceCount);
                _terrain.AddTreeInstance(tree);
                Debug.Log(_terrain.terrainData.treeInstanceCount);
            }
        }
        _terrain.Flush();
    }

    private enum ItemsID
    {
        Wood,
        Coal,
        CopperOre,
        CopperIngot,
        Stone
    }
}
