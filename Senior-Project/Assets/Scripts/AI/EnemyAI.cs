using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlowFieldGenerator;
using static PathingManager;

public class EnemyAI : AIController, IAIBasics
{
    [SerializeField]
    private PathingType _type;
    public new Vector2 Target { get => _target; set => _target = value; }
    public float _listenRadius = 10;
    public Vector2 _velocity = Vector2.zero;


    private PathInfo _lastPathGenerated = null;

    private Vector2 _movedLastFrame, _lastPosition2D;
    public new void Awake()
    {
        base.Awake();
        StartCoroutine(nameof(UpdateDirection));
        InvokeRepeating(nameof(UpdatePath), 0, _updateFrequency);
    }
    public new void Update()
    {
        if (IsDead) { Die(); return; }
        base.Update();

        if (LookForTarget(out GameObject target, _listenRadius))
        {
            SetDestination(target.transform);
            _type = PathingType.AroundObject;
        }
        if (FlowFieldFinished)
        {
            switch (_type)
            {
                case PathingType.Flow:
                    Flow();
                    break;
                case PathingType.AroundObject:
                    ExecutePath();
                    break;
                default: break;
            }
        }
        

    }
    public void ExecutePath(){
        if (Vector2.Distance(_position2D, Target) < 0.1f)
        {
            Target = _position2D;
            return;
        }

        if (_pathInfo != null && _pathInfo.cleanedPath.Count != 0)
        {
            Vector2 direction2D = (_pathInfo.cleanedPath.Peek() - _position2D).normalized;
            Vector3 direction = new(direction2D.x, -1f, direction2D.y);

            _animator.SetFloat("Speed", Vector3.Scale(direction * Speed * Time.deltaTime, new(1, 0, 1)).magnitude);
            _characterController.Move(Mathf.Clamp(Speed, 0.25f, Vector3.Distance(_position2D, Target) / Time.deltaTime) * Time.deltaTime * direction);
            if ((_pathInfo.cleanedPath.Peek() - _position2D).sqrMagnitude < 0.02f)
            {
                _pathInfo.cleanedPath.TryDequeue(out Vector2 _);
            }
        }
    }
    public void UpdatePath()
    {
        if (Target == Vector2.zero)
            return;
        float distanceToTarget = Vector2.Distance(_position2D, Target);
        if (distanceToTarget < 0.1f)
        {
            _pathInfo.cleanedPath.Clear();
            return;
        }
        if (distanceToTarget < 0.5f && _movedLastFrame.magnitude < 0.02f)
        {
            Target = _position2D;
            return;
        }
        if (_lastPathGenerated is null)
        {
            _lastPathGenerated = new PathInfo() { Start = _position2D, End = Target };

            Instance.QueuePath(_lastPathGenerated);
        }
        if (Instance.RetrievePath(_lastPathGenerated) != null)
        {
            _pathInfo = Instance.RetrievePath(_lastPathGenerated);
            _lastPathGenerated = null;
        }
    }
    public void Flow()
    {

        
        Vector3 movementDirection = new Vector3(_velocity.x, -100f, _velocity.y) * Speed * Time.deltaTime;
        Ray movementRay = new Ray(_position, _velocity);
        //if (!Physics.Raycast(movementRay, movementDirection.magnitude/4, LayerMask.GetMask("EnemyUnit")))
        //{
              _characterController.Move(movementDirection);
        //}
        //_animator.SetFloat("Speed", Vector3.Scale(movementDirection, new(1, 0, 1)).magnitude);
    }
    private IEnumerator UpdateDirection()
    {
        while (true)
        {
            Vector2Int roundedPosition = new(Mathf.RoundToInt(_position.x), Mathf.RoundToInt(_position.z));

            Vector2Int[] posToObserve = new Vector2Int[8] { new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1) };

            try
            {
                Vector2 totalDirection = FlowTiles[roundedPosition.x, roundedPosition.y].direction;
                totalDirection += Random.insideUnitCircle.normalized;
                for (int i = 0; i < posToObserve.Length; i++)
                {
                    try
                    {
                        Vector2 direction = FlowTiles[roundedPosition.x + posToObserve[i].x, roundedPosition.y + posToObserve[i].y].direction;
                        if (direction.Equals(Vector2.zero))
                        {
                            direction = _position2D - roundedPosition + posToObserve[i];
                            float mag = 1.25f / direction.magnitude;
                            direction *= -mag;
                        }

                        totalDirection += direction.normalized;
                    }
                    catch
                    {
                        Debug.Log("Outside Bounds");
                    }
                    totalDirection.Normalize();
                    _velocity += totalDirection / Random.Range(10f, 25f);
                    _velocity.Normalize();

                }
            }
            catch
            {
                Debug.Log("Enemy Outside of Bounds... Removing...");
                Destroy(gameObject);
            }
            yield return new WaitForSecondsRealtime(_updateFrequency);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((LayerMask.GetMask("PlayerUnit","Building") & hit.gameObject.layer) > 0)
        {
            Debug.Log(hit.gameObject);
        }
    }
    /**
     * <returns>Returns <c>true</c> if an object was detected otherwise <c>false</c></returns>
     * <param name="target">Returns the closest target to the Object that called it</param>
     * <param name="radius">Distance in units that it will check</param>
     */
    private bool LookForTarget(out GameObject target, float radius)
    {
        target = null;

        Collider[] possibleTargets = new Collider[100];

        if (Physics.OverlapSphereNonAlloc(position: _position, radius: radius,results: possibleTargets, layerMask: LayerMask.GetMask("PlayerUnit")) > 0)
        {
            Vector3 direction = _position - possibleTargets[0].transform.position;
            direction.z = 0.5f;
            target = possibleTargets[0].transform.gameObject;

            Debug.Log(target);
            //bool canSee = Physics.Raycast(_position, direction, radius);
            //target = canSee ? target : null;

            return true;
        }
        else
        {
            return false;
        }
    }


}
public enum PathingType
{
    Flow,
    AroundObject
}
