
using UnityEngine;

public interface IAttack
{
    abstract void Attack(GameObject target);
    abstract int DamageAmount { get; }
    abstract float AttackSpeed { get; }
    abstract float AttackCooldown { get; }
    abstract int VeteranPercent { get; }
    abstract GameObject AttackTarget { get; }
    abstract float AttackRadius { get; }


}
