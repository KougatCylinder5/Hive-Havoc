using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathInfo : IEquatable<PathInfo>, IEqualityComparer<PathInfo>
{
    public Vector2 Start;
    public Vector2 End;

    public float pathLifeTime = 10;

    public Queue<Vector2> path = new();

    public bool pathFound = false;
    
    public void CheckLifeTime()
    {
        if(pathLifeTime <= 0)
        {
            DeletePath();
        }
    }

    private void DeletePath()
    {
        PathingManager.Instance.Paths.Remove(this);
    }
    
    public bool Equals(PathInfo other)
    {
        return Start == other.Start && End == other.End;
    }

    public bool Equals(PathInfo x, PathInfo y)
    {
        return x.Start == y.Start && x.End == y.End;
    }

    public int GetHashCode(PathInfo obj)
    {
        return (Mathf.RoundToInt(obj.Start.x * 100) << 0) + (Mathf.RoundToInt(obj.Start.y * 100) << 4) + (Mathf.RoundToInt(obj.End.x * 100) << 8) + (Mathf.RoundToInt(obj.End.y * 100) << 12);
    }

    public override string ToString()
    {
        foreach (var item in path)
        {
            item.ToString();
        }
        return "";
    }


}
