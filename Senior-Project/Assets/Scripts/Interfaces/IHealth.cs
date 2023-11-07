public interface IHealth
{
    abstract void DealDamage(int damage);
    abstract void Regenerate();
    abstract int Health { get; }
    abstract int MaxHealth { get; }
    abstract int HealthRegen { get; }
    abstract float Resistance { get; }
    abstract bool IsDead { get; }
}