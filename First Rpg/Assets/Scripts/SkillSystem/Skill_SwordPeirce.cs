using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SwordPeirce : SkillObject_Sword
{
    private int amountToPierce;


    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);
        amountToPierce = swordManager.amountTopierce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if(amountToPierce <= 0||groundHit)
        {
            DamageEnemiesInRadius(transform, 0.3f);
            StopSword(collision);
        }

        amountToPierce--;
        DamageEnemiesInRadius(transform, 0.3f);
    }
}
