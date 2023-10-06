using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using static FlowFieldGenerator;
using Unity.VisualScripting;

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
    private float _updateFrequency = 0.5f, _updateTimeRemaining = 0f;
    [SerializeField]
    private PathingType _pathingType;

    public void Start()
    {
    }

    public void Update()
    {


        position = transform.position;
        position2D = new(position.x, position.z);
        switch (_pathingType)
        {
            case PathingType.Direct:
                transform.position = Vector3.MoveTowards(position, new Vector3(target.x, 1f, target.y), speed * Time.deltaTime);
                break;

            case PathingType.AroundObject:


                if ((_updateFrequency < _updateTimeRemaining || IsStale) && Vector3.Distance(position, new Vector3(target.x, 1f, target.y)) > 0.01f)
                {
                    
                    _Path = RetrieveNewPath();

                }
                if (_Path != null && _Path.cleanedPath.Count > 0)
                {

                    transform.position = Vector3.MoveTowards(position, new Vector3(_Path.cleanedPath.Peek().x, 1f, _Path.cleanedPath.Peek().y), speed * Time.deltaTime);
                    if (Vector3.Distance(position, new Vector3(_Path.cleanedPath.Peek().x, 1f, _Path.cleanedPath.Peek().y)) < 0.01f)
                    {
                        _Path.cleanedPath.Dequeue();
                    }
                }
                else if (_Path != null && Vector3.Distance(position, new Vector3(_Path.End.x, 1f, _Path.End.y)) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(position, new Vector3(_Path.End.x, 1f, _Path.End.y), speed * Time.deltaTime);
                }
                break;

            case PathingType.Flow:
                Vector2Int roundedPosition = new(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));

                Vector2Int[] posToObserve = new Vector2Int[8] { new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1) };

                totalDirection = new();

                totalDirection = FlowTiles[roundedPosition.x, roundedPosition.y].direction;

                for (int i = 0; i < posToObserve.Length; i++)
                {
                    try
                    {
                        Vector2 direction = FlowTiles[roundedPosition.x + posToObserve[i].x, roundedPosition.y + posToObserve[i].y].direction;
                        if (direction.Equals(Vector2.zero))
                        {
                            direction = FlowTiles[roundedPosition.x + posToObserve[i].x, roundedPosition.y + posToObserve[i].y].position - position2D;
                            direction.Normalize();
                            direction /= 2;
                        }

                        totalDirection += direction;
                    }
                    catch
                    {
                        Debug.Log("Outside Bounds");
                    }
                }


                totalDirection.Normalize();


                transform.Translate(new Vector3(totalDirection.x * Time.deltaTime, 0.0f, totalDirection.y * Time.deltaTime) * speed);

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
