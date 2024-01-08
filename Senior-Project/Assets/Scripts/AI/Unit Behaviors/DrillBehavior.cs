using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrillBehavior : UnitAI
{
    public Collider[] enemies = new Collider[100];

    private new void Awake()
    {
        base.Awake();
        DamageAmount = 10;
        AttackCooldown = 0;
        AttackSpeed = 1;
        AttackBaseCooldown = 1.5f;
    }
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        // check for enemies in range to attack
        if (Physics.OverlapSphereNonAlloc(_position, AttackRadius, enemies, LayerMask.GetMask("EnemyUnit", "EnemyBuilding")) > 0 && !IsIgnoringEnemies)
        {
            enemies = enemies.OrderBy(c => { if(c && !c.GetComponent<IHealth>().IsDead) return (c.transform.position - _position).magnitude; return float.PositiveInfinity; }).ToArray();
            AttackTarget = enemies[0].gameObject;
            Vector3 enemyAtHeight = AttackTarget.transform.position;
            enemyAtHeight.y = _position.y;
            Quaternion endRotation = Quaternion.LookRotation((enemyAtHeight - _position).normalized);
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, endRotation, 180 * Time.deltaTime);
        }
        else
        {
            AttackTarget = null;
        }
        if (AttackTarget && !IsIgnoringEnemies && AttackCooldown <= 0)
        {
            Attack(AttackTarget);
            AttackCooldown = AttackBaseCooldown;
        }// if moving can't attack enemies
        else if(IsIgnoringEnemies)
        {
            ExecutePath();
        }
        if (AttackCooldown >= 0)
        {
            AttackCooldown -= Time.deltaTime*AttackSpeed;
        }
    }

    private new void Attack(GameObject target)
    {
        target.GetComponent<IHealth>().DealDamage(DamageAmount);
    }




    
}
