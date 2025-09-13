using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice blast", fileName = "Item effect data - Ice balst on taking damage")]
public class ItemEffect_iceBlastOnTakingDamage : ItemEffect_DataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [Space]
    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    private float lastTimeUsed=-999;
    [Header("Vfx Object")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    public override void ExecuteEffect()
    {
       bool noCooldowm = Time.time>=lastTimeUsed+cooldown;
       bool reachedThreshold = player.health.GetHeakthPercent()<=healthPercentTrigger;

        if (noCooldowm && reachedThreshold)
        {
            player.vfx.CreateEffectof(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach(var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if(damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if (targetGotHit)
                player.vfx.CreateEffectof(onHitVfx, target.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTackingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe(); 
        player.health.OnTackingDamage -= ExecuteEffect;
        player = null;
    }
}
