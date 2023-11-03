using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HunterBehavior : UnitAI
{

    private void Awake()
    {
        base.Awake();
        DamageAmount = 10;
        AttackCooldown = 0;
        AttackSpeed = 1;
    }
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        Collider[] enemies = new Collider[10];


        if (Physics.OverlapSphereNonAlloc(_position, AttackRadius, enemies, LayerMask.GetMask("EnemyUnit")) > 0)
        {
            enemies.OrderBy(c => { if(c) return (_position - c.transform.position).sqrMagnitude; return float.PositiveInfinity; }).ToArray();
            AttackTarget = enemies[0].gameObject;
            if (AttackTarget != null)
                Target = new(AttackTarget.transform.position.x, AttackTarget.transform.position.z);

        }
        if (AttackTarget != null && Vector3.Distance(_position, AttackTarget.transform.position) > AttackRadius)
        {
            ExecutePath();
        }
        else if (AttackTarget != null)
        {
            if(AttackCooldown > AttackSpeed)
            {
                Attack(AttackTarget);
                AttackCooldown = 0;
            }
            else
            {
                AttackCooldown += Time.deltaTime;
            }
            
        }
        if(AttackTarget == null)
        {
            ExecutePath();
        }
        

    }


    private new void Attack(GameObject target)
    {
        target.GetComponent<EnemyAI>().DealDamage(DamageAmount);
    }
}
