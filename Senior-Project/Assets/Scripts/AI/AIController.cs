using System.Linq;
using Unity.Burst;
using UnityEngine;

using static PathingManager;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class AIController : IHealth
{

    public Vector2 Target { get => _target; protected set => _target = value; }

    [SerializeField]
    protected Vector2 _target;
    protected PathInfo _pathInfo;

    public Vector3 Position { get => _position; protected set => _position = value; }
    public Vector2 Position2D { get => _position2D; protected set => _position2D = value; }

    [SerializeField]
    protected Vector3 _position;
    protected Vector2 _position2D;
    [SerializeField]
    private float speed;

    public float Speed { get => speed; protected set => speed = value; }

    [SerializeField]
    protected float _updateFrequency = 0.1f;

    [SerializeField]
    protected CharacterController _characterController;
    protected Animator _animator;
    [SerializeField]
    protected int _health, _maxHealth;
    
    protected int _regeneration;
    protected float _resistance;
    [SerializeField]
    private bool _isDead = false;
    protected bool _isIgnoringEnemies = false;
    public override bool IsDead { get => _isDead; protected set => _isDead = value; }
    public override int Health
    {
        get => _health;
        protected set
        {
            if (value <= 0)
            {
                _health = 0;
                _isDead = true;
            }
            else
            {
                _health = value;
            }
        }
    }
    public override int MaxHealth { get => _maxHealth; protected set => _maxHealth = value; }

    public override int HealthRegen { get => _regeneration; protected set => _regeneration = value; }

    public override float Resistance { get => _resistance; protected set => _resistance = value; }

    public bool IsIgnoringEnemies { get => _isIgnoringEnemies; set => _isIgnoringEnemies = value; }

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
        
        if (_pathInfo.cleanedPath.TryPeek(out Vector2 nextNode))
        {
            transform.LookAt(PathInfo.ConvertToVector3(nextNode, _position.y));
        }
        else
        {
            _isIgnoringEnemies = false;
        }
    }

    protected void Die()
    {
        //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        //{
        //_animator.SetTrigger("Death");
        UnitController.RemoveUnit(gameObject);
        Destroy(gameObject, 0.5f);
        //Invoke(nameof(DestroySelf), 0.5f);
        //}
    }
    private void DestroySelf()
    {
        Destroy(gameObject,0.5f);
    }
    public override void DealDamage(int damage)
    {
        Health -= Mathf.RoundToInt(damage * (1 - _resistance));
    }
    public override void Regenerate()
    {
        Health += _regeneration;
    }
    public override void SetHealth(int health)
    {
        Health = health;
    }
}