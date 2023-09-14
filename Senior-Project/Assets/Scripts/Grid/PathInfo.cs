using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathInfo
{
    public Vector2 Start;
    public Vector2 End;

    public Queue<Vector2> path;
}
