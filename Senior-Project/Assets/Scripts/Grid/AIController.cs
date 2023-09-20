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
    private int _pathQueuePosition = -1;
    private float _updateFrequency = 0.5f;

    public void Start()
    {
        target = new Vector2(10, 10);
        
    }

    public void Update()
    {
        position = transform.position;
        requestedPath = new PathInfo() { Start = position, End = target };
        PathingManager.Instance.QueuePath(requestedPath);
    }


    public void LateUpdate()
    {

        path = PathingManager.Instance.Paths.ElementAt(PathingManager.Instance.Paths.IndexOf(requestedPath));
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
