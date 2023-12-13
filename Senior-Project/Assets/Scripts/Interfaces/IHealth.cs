using UnityEngine;

public abstract class IHealth : MonoBehaviour
{
    public abstract void SetHealth(int health);
    public abstract void DealDamage(int damage);
    public abstract void Regenerate();
    public abstract int Health { get; protected set; }
    public abstract int MaxHealth { get; protected set; }
    public abstract int HealthRegen { get; protected set; }
    public abstract float Resistance { get; protected set; }
    public abstract bool IsDead { get; protected set; }
}