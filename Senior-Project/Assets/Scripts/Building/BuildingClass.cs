using UnityEngine;

public class BuildingClass : IHealth
{
    
    [SerializeField]
    protected int _health, _maxHealth, _regeneration;
    [SerializeField]
    private bool _isDead = false;
    public override bool IsDead { get => _isDead; protected set => _isDead = value; }
    protected float _resistance;

    public override int Health {
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
    public override int MaxHealth { get => _maxHealth; protected set => _maxHealth = value; }
    public override int HealthRegen { get => _regeneration; protected set => _regeneration = value; }
    public override float Resistance { get => _resistance; protected set => _resistance = value; }

    public override void DealDamage(int damage)
    {
        Health -= Mathf.RoundToInt(damage * (1-_resistance));
        print("attacked");
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
