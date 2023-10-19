using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlowFieldGenerator;
public class EnemyAI : AIController, IAIBasics
{
    [SerializeField]
    private PathingType _type;
    public Vector2 Target { get => _target; set => _target = value; }
    public float _listenRadius = 10;
    public Vector2 _velocity = Vector2.zero;
    
    public new void Awake()
    {
        base.Awake();
    }
    public new void FixedUpdate()
    {
        base.FixedUpdate();

        //if (LookForTarget(out GameObject target, _listenRadius))
        //{
        //    SetDestination(target.transform);
        //    _type = PathingType.AroundObject;
        //}
        //else
        //{
        //    _type = PathingType.Flow;
        //    _target = _startPoint;
        //}
        if (_type == PathingType.Flow) Flow();
        if (_type == PathingType.AroundObject) ExecutePath();

    }
    public void ExecutePath()
    {

    }
    public void Flow()
    {
        
        Vector2Int roundedPosition = new(Mathf.RoundToInt(_position.x), Mathf.RoundToInt(_position.z));

        Vector2Int[] posToObserve = new Vector2Int[8] { new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1) };

        Vector2 totalDirection = FlowTiles[roundedPosition.x, roundedPosition.y].direction;
        totalDirection += Random.insideUnitCircle.normalized * 5;
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
        }


        totalDirection.Normalize();
        _velocity += totalDirection / Random.Range(10f, 25f);
        _velocity.Normalize();

        _characterController.Move(new Vector3(_velocity.x * Time.fixedDeltaTime, -1.0f, _velocity.y * Time.fixedDeltaTime) * speed);
    }

    /**
     * <returns>Returns <c>true</c> if an object was detected otherwise <c>false</c></returns>
     * <param name="target">Returns the closest target to the Object that called it</param>
     */
    private bool LookForTarget(out GameObject target, float radius)
    {
        target = null;
        if (Physics.SphereCast(origin: _position, radius: radius, direction: Vector3.zero, hitInfo: out RaycastHit hit, maxDistance: radius, layerMask: LayerMask.GetMask("PlayerUnit")))
        {
            Vector3 direction = _position - hit.point;
            direction.z = 0.5f;

            bool canSee = Physics.Raycast(_position, direction, radius);
            target = canSee ? target : null;

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
