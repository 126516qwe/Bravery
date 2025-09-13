using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject onHitVfx;
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1f;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;
    protected bool targerGotHit;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected void DamageEnemiesInRadius(Transform t,float radius)
    {
        foreach(var target in GetEnemiesAround(t, radius))
        {
            IDamagable damagable= target.GetComponent<IDamagable>();

            if(damagable==null)
                continue;

            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float phyDamage = attackData.phyiscalDamage;
            float elemDamage =attackData.elementalDamage;
            ElementType element = attackData.element;

            targerGotHit= damagable.TakeDamage(phyDamage, elemDamage,element, transform);

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targerGotHit)
            {
                lastTarget = target.transform;
                Instantiate(onHitVfx, target.transform.position, Quaternion.identity); 
            }

            usedElement = element;
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closesDistance = Mathf.Infinity;

        foreach(var enemy in GetEnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closesDistance)
            {
                target = enemy.transform;
                closesDistance = distance;
            }
        }

        return target;  
    }

    protected Collider2D[] GetEnemiesAround(Transform t,float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }
    
    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;    

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
