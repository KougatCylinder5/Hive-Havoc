using System.Linq;
using Unity.Burst;
using UnityEngine;

using static PathingManager;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class AIController : MonoBehaviour
{
    [SerializeField]
    protected Vector2 _target;
    protected PathInfo _pathInfo;

    [SerializeField]
    protected Vector3 _position;
    protected Vector2 _position2D;
    public float speed;

    [SerializeField]
    protected float _updateFrequency = 0.1f;

    [SerializeField]
    protected CharacterController _characterController;
    protected Animator _animator;

    public void Awake()
    {
        _pathInfo = new();
        _characterController = GetComponent<CharacterController>();
        _position = transform.position;
        _position2D = new(_position.x, _position.z);
        _target = _position2D;
        _animator = GetComponent<Animator>();
        _animator.SetFloat("Randomness", Random.Range(0, 1f));
        
    }
    public bool SetDestination(Vector2 target)
    {
        bool pathable = IsOpen(target);

        if (pathable)
        {
            _target = target;
        }
        return pathable;

    }
    public bool SetDestination(Transform target)
    {
        Vector2 target2D = new(target.position.x, target.position.z);
        bool pathable = IsOpen(target2D);

        if (pathable)
        {
            _target = target2D;
        }
        return pathable;
    }

    public void Update()
    {
        _position = transform.position;
        _position2D = new(_position.x, _position.z);

        try
        {
            if (_pathInfo.cleanedPath.TryPeek(out Vector2 nextNode))
            {
                transform.LookAt(PathInfo.ConvertToVector3(nextNode, _position.y));
            }
        }
        catch{ }
            

        
    }

    //[BurstCompile]
    //public void FixedUpdate()
    //{
    //    

    //    switch (_pathingType)
    //    {
    //        case PathingType.AroundObject:

    //            if (_updateFrequency > _updateTimeRemaining && Vector2.Distance(_position2D, _target) > 0.1f)
    //            {
    //                _Path = RetrieveNewPath();
    //                _updateTimeRemaining += Time.fixedDeltaTime;
    //            }
    //            else
    //            {
    //                _updateTimeRemaining = 0;
    //            }
    //            if (_Path != null && _Path.cleanedPath.TryPeek(out Vector2 pathTarget))
    //            {

    //                Vector2 movementVector = pathTarget - _position2D;
    //                movementVector.Normalize();
    //                if (Vector2.Distance(_position2D, pathTarget) > 0.1f)
    //                {
    //                    _characterController.Move(new Vector3(movementVector.x, -1f, movementVector.y) * speed * Time.fixedDeltaTime);
    //                }
    //                else
    //                {
    //                    _Path.cleanedPath.Dequeue();
    //                }
    //            }


    //            break;

    //        case PathingType.Flow:
    //            


    //    }


    //}


    //public void LateUpdate()
    //{
    //    if (_pathingType == PathingType.AroundObject)
    //    {
    //        RequestNewPath();
    //    }
    //}

    //private void RequestNewPath()
    //{
    //    if ((_updateFrequency < _updateTimeRemaining || IsStale) && Vector3.Distance(_position, new Vector3(_target.x, 1f, _target.y)) > 0.01f)
    //    {
    //        _requestedPath = new PathInfo() { Start = new(_position.x, _position.z), End = _target };
    //        PathingManager.Instance.QueuePath(_requestedPath);
    //        _updateTimeRemaining = 0;
    //    }
    //    else
    //    {
    //        _updateTimeRemaining += Time.deltaTime;
    //    }
    //}


    //private PathInfo RetrieveNewPath()
    //{
    //    int pathPos = PathingManager.Instance.Paths.IndexOf(_requestedPath);
    //    if (pathPos != -1)
    //    {
    //        return PathingManager.Instance.Paths.ElementAt(pathPos);
    //    }
    //    return _Path;

    //}

    //public bool SetDestination(Vector2 destination)
    //{

    //    bool canPath = PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(destination.x), Mathf.RoundToInt(destination.y))];
    //    if (canPath)
    //    {
    //        this._target = destination;
    //        _pathingType = PathingType.AroundObject;
    //    }
    //    return canPath;
    //}

    //public bool SetTarget(Transform target)
    //{
    //    Vector2 temp = new Vector2(target.position.x, target.position.z);

    //    bool canPath = PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(target.position.x), Mathf.RoundToInt(target.position.z))];

    //    if (canPath)
    //    {
    //        this._target = temp;
    //        _pathingType = PathingType.AroundObject;
    //    }
    //    return canPath;

    //}
}