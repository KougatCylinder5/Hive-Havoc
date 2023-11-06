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
        raycastLayers = LayerMask.GetMask(new string[] {"Water", "Building", "Terrain", "Trees" });
    }

    public void CleanPath()
    {
        Queue<Vector2> copyPath = new Queue<Vector2>(path);
        copyPath.Enqueue(End);

        Vector2 curNode = Start;

        cleanedPath.Clear();

        if (copyPath.Count > 0)
        {
            Vector2 priorNode = curNode;

            while (copyPath.Count > 0)
            {
                Vector3 center = ConvertToVector3(curNode - (curNode.normalized-Start.normalized/2).normalized, 0.85f);
                Vector3 halfExtends = Vector3.one / 4f;
                halfExtends.y = 0;
                Vector3 direction = ConvertToVector3(copyPath.Peek() - curNode, 0).normalized;
                if (Physics.SphereCast(origin: center, radius: 0.25f, direction: direction, maxDistance: (copyPath.Peek() - curNode).magnitude, layerMask: raycastLayers, hitInfo: out RaycastHit hit)) 
                {
                    cleanedPath.Enqueue(priorNode);
                }
                priorNode = copyPath.Dequeue();

            }
            if (!priorNode.Equals(new Vector2(-1, -1)))
                cleanedPath.Enqueue(priorNode);
        }
        
    }

    public static Vector3 ConvertToVector3(Vector2 obj, float height)
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
        return Mathf.RoundToInt(obj.Start.x)+ Mathf.RoundToInt(obj.Start.y * 100) + Mathf.RoundToInt(obj.End.x * 10000) + Mathf.RoundToInt(obj.End.y * 1000000);
    }

    public override string ToString()
    {
        string pathString = string.Empty;

        foreach (var item in path)
        {
            pathString += ", " + item.ToString();
        }
        return pathString;
    }


}
