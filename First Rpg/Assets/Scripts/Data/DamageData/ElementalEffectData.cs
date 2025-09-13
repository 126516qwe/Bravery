
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementalEffectData 
{
    public float chillDuration;
    public float chillSlowMultiplier;

    public float burnDuration;
    public float totalBurnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCherge;

    public ElementalEffectData(Entity_Stats entityStats,DamageScaleData damageScale) 
    {
        chillDuration = damageScale.chillDuration;
        chillSlowMultiplier =damageScale.chillSlowMuliplier;

        burnDuration = damageScale.burnDuration;
        totalBurnDamage = entityStats.offense.lightningDamage.GetValue() * damageScale.burnDamageScale;

        shockDuration = damageScale.shockDuration;
        shockDamage = entityStats.offense.lightningDamage.GetValue()*damageScale.shockDamageScale;
        shockCherge = damageScale.shockCharge;

    }
}




