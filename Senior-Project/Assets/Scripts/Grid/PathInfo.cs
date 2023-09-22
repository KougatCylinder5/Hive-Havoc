using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathInfo : IEquatable<PathInfo>, IEqualityComparer<PathInfo>
{
    public Vector2 Start;
    public Vector2 End;

    public Queue<Vector2> path = new();

    public bool pathFound = false;

    private LayerMask raycastLayers;

    public Queue<Vector2> cleanedPath = new();

    public PathInfo()
    {
        raycastLayers = ~LayerMask.GetMask(new string[] { "Building", "Terrain" });
    }

    public void CleanPath()
    {
        Queue<Vector2> copyPath = path;
        Vector2 curNode = copyPath.Dequeue();


        while(copyPath.Count > 0)
        {
            Ray ray = new Ray(ConvertToVector3(copyPath.Peek(), 0.1f), ConvertToVector3(copyPath.Peek(), 0.1f) - ConvertToVector3(curNode, 0.1f));
            
            if (Physics.Raycast())
            {

            }
        }


    }

    private Vector3 ConvertToVector3(Vector2 obj, float height)
    {
        return new Vector3(obj.x, height, obj.y);
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
