using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSelection : MonoBehaviour
{
    private Vector2 _firstPos = Vector2.zero;
    private Vector2 _lastPos = Vector2.zero;
    [SerializeField]
    private LayerMask _unitMask;
    Vector3 halfExtend;
    Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _firstPos = Mouse.MouseToWorldPoint();
        }
        if (Input.GetMouseButtonUp(0))
        {
            _lastPos = Mouse.MouseToWorldPoint();
        }
        if(_firstPos != Vector2.zero && _lastPos != Vector2.zero)
        {
            Vector2 center2D = (_firstPos + _lastPos) / 2;
            center = new(center2D.x, -2, center2D.y);
            Vector2 halfExtend2D = (_lastPos - _firstPos) / 2;
            halfExtend = new Vector3(halfExtend2D.x, 10, halfExtend2D.y);
            if (halfExtend.y < 0)
            {
                halfExtend = -halfExtend;
            }

            RaycastHit[] hits = Physics.BoxCastAll(center, halfExtend, Vector3.up, Camera.main.transform.rotation/*, ~_unitMask*/);
            //_firstPos = null;
            //_lastPos = null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(center,halfExtend);
    }
}
