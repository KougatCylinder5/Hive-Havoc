using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using static FlowFieldGenerator;
using System.Data.SqlTypes;

public class AIController : MonoBehaviour
{

    [SerializeField]
    private Vector2 target;
    private Vector3 position;
    private Vector2 position2D;
    public float speed;
    public Vector2 totalDirection;

    public bool IsStale { get; private set; }

    private PathInfo _Path, _requestedPath;
    [SerializeField]
    private float _updateFrequency = 0.1f, _updateTimeRemaining = 0f;
    [SerializeField]
    private PathingType _pathingType;

    public Vector2 velocity;

    [SerializeField]
    private CharacterController _characterController;


    public void Start()
    {
        position = transform.position;
        position2D = new(position.x, position.z);
        target = position2D;
    }

    public void FixedUpdate()
    {


        position = transform.position;
        position2D = new(position.x, position.z);
        switch (_pathingType)
        {
            case PathingType.Direct:
                _characterController.Move(new Vector3(target.x, -1f, target.y)* speed * Time.fixedDeltaTime);
                break;

            case PathingType.AroundObject:

                if(_updateFrequency > _updateTimeRemaining && Vector2.Distance(position2D, target) > 0.1f)
                {
                    _Path = RetrieveNewPath();
                    _updateTimeRemaining += Time.fixedDeltaTime;
                }
                else
                {
                    _updateTimeRemaining = 0;
                }
                if(_Path != null && _Path.cleanedPath.TryPeek(out Vector2 pathTarget))
                {
                    Vector2 movementVector = pathTarget - position2D;
                    movementVector.Normalize();
                    if(Vector2.Distance(position2D, pathTarget) > 0.1f)
                    {
                        _characterController.Move(new Vector3(movementVector.x, -1f, movementVector.y) * speed * Time.fixedDeltaTime);
                    }
                    else
                    {
                        _Path.cleanedPath.Dequeue();
                    }
                }


                break;

            case PathingType.Flow:
                Vector2Int roundedPosition = new(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));

                Vector2Int[] posToObserve = new Vector2Int[8] { new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1) };

                totalDirection = new();

                totalDirection = FlowTiles[roundedPosition.x, roundedPosition.y].direction;
                totalDirection += Random.insideUnitCircle.normalized * 5;
                for (int i = 0; i < posToObserve.Length; i++)
                {
                    try
                    {
                        Vector2 direction = FlowTiles[roundedPosition.x + posToObserve[i].x, roundedPosition.y + posToObserve[i].y].direction;
                        if (direction.Equals(Vector2.zero))
                        {
                            direction = position2D - roundedPosition + posToObserve[i];
                            float mag = 1.25f / direction.magnitude;
                            direction *= -mag;
                            
                        }

                        totalDirection += direction.normalized;
                    }
                    catch
                    {
                        Debug.Log("Outside Bounds");
                    }
                }
                

                totalDirection.Normalize();
                velocity += totalDirection / Random.Range(10f,25f);
                velocity.Normalize();

                _characterController.Move(new Vector3(velocity.x * Time.fixedDeltaTime, -1.0f, velocity.y * Time.fixedDeltaTime) * speed);


                break;


        }


    }


    public void LateUpdate()
    {
        if (_pathingType == PathingType.AroundObject)
        {
            RequestNewPath();
        }
    }

    private void RequestNewPath()
    {
        if ((_updateFrequency < _updateTimeRemaining || IsStale) && Vector3.Distance(position, new Vector3(target.x, 1f, target.y)) > 0.01f)
        {
            _requestedPath = new PathInfo() { Start = new(position.x, position.z), End = target };
            PathingManager.Instance.QueuePath(_requestedPath);
            _updateTimeRemaining = 0;
        }
        else
        {
            _updateTimeRemaining += Time.deltaTime;
        }
    }


    private PathInfo RetrieveNewPath()
    {
        int pathPos = PathingManager.Instance.Paths.IndexOf(_requestedPath);
        if (pathPos != -1)
        {
            return PathingManager.Instance.Paths.ElementAt(pathPos);
        }
        return _Path;

    }

    public bool SetDestination(Vector2 destination)
    {
        
        bool canPath = PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(destination.x), Mathf.RoundToInt(destination.y))];
        if (canPath)
        {
            this.target = destination;
            _pathingType = PathingType.AroundObject;
        }
        return canPath;
    }

    public bool SetTarget(Transform target)
    {
        Vector2 temp = new Vector2(target.position.x, target.position.z);

        bool canPath = PathingManager.ObstructedTiles[PathingManager.CalculateIndex(Mathf.RoundToInt(target.position.x), Mathf.RoundToInt(target.position.z))];

        if(canPath)
        {
            this.target = temp;
            _pathingType = PathingType.AroundObject;
        }
        return canPath;

    }




}

public enum PathingType
{
    Flow,
    Direct,
    AroundObject
}
