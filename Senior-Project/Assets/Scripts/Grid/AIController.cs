using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class AIController : MonoBehaviour
{

    [SerializeField]
    private Vector2 target, position;
    
    public float speed;
    public bool IsStale { get; private set; }

    private PathInfo path, requestedPath;
    [SerializeField]
    private float _updateFrequency = 0.5f;
    [SerializeField]
    private PathingType _pathingType;

    public void Start()
    {
        target = new Vector2(10, 10);
        
    }

    public void Update()
    {
        switch (_pathingType)
        {
            case PathingType.Direct:


                break;

            case PathingType.AroundObject:


                break;

            case PathingType.Flow:


                break;


        }
        position = transform.position;
        requestedPath = new PathInfo() { Start = position, End = target };
        PathingManager.Instance.QueuePath(requestedPath);
    }


    public void LateUpdate()
    {
        int pathPos = PathingManager.Instance.Paths.IndexOf(requestedPath);
        if (pathPos != -1)
        {
            path = PathingManager.Instance.Paths.ElementAt(pathPos);
        }
    }

    private void RequestNewPath()
    {
        
    }

    public void SetDestination(Vector2 destination)
    {
        target = destination;
    }

    public void SetTarget(ref Transform target)
    {
        this.target = target.position;
    }


    

}

public enum PathingType
{
    Flow,
    Direct,
    AroundObject
}
