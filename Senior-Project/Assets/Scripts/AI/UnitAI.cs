using System.Collections;
using UnityEngine;
using static PathingManager;

[RequireComponent(typeof(LineRenderer))]
public class UnitAI : AIController, IAIBasics, IAttack
{
    public new Vector2 Target { get => _target; set => _target = value; }
    
    private PathInfo _lastPathGenerated = null;

    private Vector2 _movedLastFrame, _lastPosition2D;

    private LineRenderer _pathRenderer;

    private float _attackSpeed;
    public float AttackSpeed { get => _attackSpeed; protected set => _attackSpeed = value; }

    private float _attackCooldown;
    public float AttackCooldown { get => _attackCooldown; protected set => _attackCooldown = value; }

    private float _attackBaseCooldown;
    public float AttackBaseCooldown { get => _attackBaseCooldown; protected set => _attackBaseCooldown = value; }

    private int _veteranPercent;
    public int VeteranPercent { get => _veteranPercent; protected set => _veteranPercent = value; }

    private int _damageAmount;
    public int DamageAmount { get => _damageAmount; protected set => _damageAmount = value; }
    [SerializeField]
    private GameObject _attackTarget;
    public GameObject AttackTarget { get => _attackTarget; protected set => _attackTarget = value; }
    [SerializeField]
    private float _attackRadius;
    public float AttackRadius { get => _attackRadius; protected set => _attackRadius = value; }

    public new void Awake()
    {
        base.Awake();
        _pathRenderer = GetComponent<LineRenderer>();
        _pathRenderer.endWidth = 0.0f;
        _pathRenderer.startWidth = 0.3f;
        InvokeRepeating(nameof(UpdatePath), 0, _updateFrequency);
        _lastPosition2D = _position2D;
        StartCoroutine(nameof(DistanceMoved));
    }
    
    public void ExecutePath()
    {
        if (Vector2.Distance(_position2D, Target) < 0.1f)
        {
            Target = _position2D;
            return;
        }
        print(_pathInfo.cleanedPath.Count);
        if (_pathInfo.cleanedPath.Count > 0)
        {
            Vector2 direction2D = (_pathInfo.cleanedPath.Peek() - _position2D).normalized;
            Vector3 direction = new(direction2D.x, -1f, direction2D.y);

            //_animator.SetFloat("Speed", Vector3.Scale(direction * Speed * Time.deltaTime, new(1, 0, 1)).magnitude);
            _characterController.Move(Mathf.Clamp(Speed,0.25f,Vector3.Distance(_position2D, Target) / Time.deltaTime) * Time.deltaTime * direction);
            if ((_pathInfo.cleanedPath.Peek() - _position2D).sqrMagnitude < 0.02f)
            {
                _pathInfo.cleanedPath.TryDequeue(out Vector2 _);
            }
        }
    }

    public new void Update()
    {
        if (IsDead) { Die(); return; }
        base.Update();
        DisplayLine();
    }

    public void UpdatePath()
    {
        if (!Saver.LoadDone)
            return;
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
        print("looping");
    }


    private void DisplayLine()
    {
        _pathRenderer.positionCount = 2;
        _pathRenderer.SetPosition(0, _position);
        if(_pathInfo.cleanedPath.TryPeek(out Vector2 nextPoint))
        {
            _pathRenderer.SetPosition(1, PathInfo.ConvertToVector3(nextPoint, _position.y));
        }
        _pathRenderer.Simplify(0.5f);
    }

    private IEnumerator DistanceMoved()
    {
        while (true)
        {
            _movedLastFrame = _position2D - _lastPosition2D;
            _lastPosition2D = _position2D;
            yield return new WaitForSeconds(_updateFrequency * 2);
            Debug.Log(gameObject);
        }
    }

    public void Attack(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}

