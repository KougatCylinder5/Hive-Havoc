using UnityEngine;

public class BuildingClass : MonoBehaviour, IHealth
{
    [SerializeField]
    protected int maxHealth;
    protected int health;

    protected int _health, _maxHealth, _regeneration;
    private bool _isDead = false;
    public bool IsDead { get => _isDead; protected set => _isDead = value; }
    protected float _resistance;

    public int Health {
        get => _health;
        protected set
        {
            if (_health < 0)
            {
                _health = Mathf.RoundToInt(_maxHealth * 0.3f);
                IsDead = true;
                Destroy(gameObject);
            }
            else
            {
                _health = value;
            }
        }
    }
    public int MaxHealth { get => _maxHealth; }
    public int HealthRegen { get => _regeneration; }
    public float Resistance { get => _resistance; }

    public void DealDamage(int damage)
    {
        Health -= Mathf.RoundToInt(damage * (1-_resistance));
    }

    public void Regenerate()
    {
        Health += _regeneration;
    }
}
