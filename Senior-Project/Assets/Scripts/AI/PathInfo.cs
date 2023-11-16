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
        Queue<Vector2> copyPath = new(path);
        copyPath.Enqueue(End);

        Vector2 curNode = Start;

        


        cleanedPath.Clear();

        Vector2 priorNode = curNode;
        Vector3 center;
        Vector3 halfExtends;
        halfExtends.y = 0;
        Vector3 direction;

        if (copyPath.Count > 0)
        {
            Vector2 nextNode = copyPath.Dequeue();

            while (copyPath.Count > 0)
            {
                center = ConvertToVector3(curNode, 0.35f);
                halfExtends = Vector3.one / 4f;
                halfExtends.y = 0;
                direction = ConvertToVector3(nextNode - curNode, 0).normalized;
                if (Physics.SphereCast(origin: center, radius: 0.25f, direction: direction, maxDistance: (nextNode - curNode).magnitude, layerMask: raycastLayers, hitInfo: out RaycastHit _)) 
                {
                    cleanedPath.Enqueue(priorNode);
                }
                priorNode = copyPath.Dequeue();

            }
            if (!priorNode.Equals(new Vector2(-1, -1)))
                cleanedPath.Enqueue(priorNode);

            center = ConvertToVector3(curNode, 0.35f);
            halfExtends = Vector3.one / 4f;
            halfExtends.y = 0;
            direction = ConvertToVector3(nextNode - End, 0).normalized;

            if (Physics.SphereCast(origin: center, radius: 0.25f, direction: direction, maxDistance: (nextNode - End).magnitude, layerMask: raycastLayers, hitInfo: out RaycastHit _))
            {
                //Debug.Log(cleanedPath.Dequeue());
                
            }
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
