using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : AIController, IAIBasics
{
    private PathingType _type;
    public Vector2 Target { get => _target; set => _target = value; }

    public void ExecutePath()
    {

    }
    
    private void FixedUpdate()
    {
        if(_type == PathingType.Flow) return;



    }
}
public enum PathingType
{
    Flow,
    AroundObject
}
