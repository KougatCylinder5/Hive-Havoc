using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(LineRenderer))]
public class UnitAI : AIController, IAIBasics
{
    public Vector2 Target { get => _target; set => _target = value; }

    private LineRenderer _pathRenderer;

    public new void Awake()
    {
        base.Awake();
        _pathRenderer = GetComponent<LineRenderer>();
        _pathRenderer.endWidth = 0.3f;
        _pathRenderer.startWidth = 0.1f;
    }

    public void ExecutePath()
    {
        
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        DisplayLine();
        ExecutePath();
    }

    private void DisplayLine()
    {
        _pathRenderer.positionCount = 2;
        _pathRenderer.SetPosition(0, _position);
        _pathRenderer.SetPosition(1, PathInfo.ConvertToVector3(_target,_position.y));
        _pathRenderer.Simplify(0.1f);
        
    }
}

