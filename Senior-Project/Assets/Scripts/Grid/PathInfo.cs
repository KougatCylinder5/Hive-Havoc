using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathInfo
{
    public Vector2Int Start;
    public Vector2Int End;

    public Queue<Vector2> path;
}
