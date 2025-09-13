using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity_Combat entityCombat;
    private Entity entity;


    protected virtual void Awake()
    {
        entityCombat = GetComponentInParent<Entity_Combat>();
        entity=GetComponentInParent<Entity>();
    }
    private void CurrentStateTrigger()
    {//需要了解当前状态并通知当前状态退出
        entity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }

}
