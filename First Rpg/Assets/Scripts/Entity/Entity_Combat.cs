
using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    private Entity_SFX sfx;
    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData basicAttackScale;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;


    private void Awake()
    {
        sfx = GetComponent<Entity_SFX>();
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }
    public void PerformAttack()
    {
        bool targetGotHit = false;

        foreach (var target in GetDetectedColliders(whatIsTarget))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            
            if (damagable == null)
                continue;
           
            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            ElementalEffectData effectData = new ElementalEffectData(stats, basicAttackScale);

            float physicalDamage = attackData.phyiscalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            targetGotHit = damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                target?.GetComponent<Entity_StatusHandler>()?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
            {
                OnDoingPhysicalDamage?.Invoke(physicalDamage);
                vfx.CreateOnHitVFX(target.transform, attackData.isCrit, element);
                sfx?.PlayAttackHit();
            }

        }
        if (targetGotHit == false)
            sfx?.PlayAttackMiss();
    }

    public void PerformAttackOnTarget(Transform target)
    {
        bool targetGotHit = false;


        IDamagable damagable = target.GetComponent<IDamagable>();

        if (damagable == null)
            return;

        AttackData attackData = stats.GetAttackData(basicAttackScale);
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

        ElementalEffectData effectData = new ElementalEffectData(stats, basicAttackScale);

        float physicalDamage = attackData.phyiscalDamage;
        float elementalDamage = attackData.elementalDamage;
        ElementType element = attackData.element;

        targetGotHit = damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);

        if (element != ElementType.None)
            target?.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, attackData.effectData);

        if (targetGotHit)
        {
            OnDoingPhysicalDamage?.Invoke(physicalDamage);
            vfx.CreateOnHitVFX(target.transform, attackData.isCrit, element);
            sfx?.PlayAttackHit();
        }


        if (targetGotHit == false)
            sfx?.PlayAttackMiss();
    }

    protected Collider2D[] GetDetectedColliders(LayerMask whatToDetect)
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatToDetect);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
