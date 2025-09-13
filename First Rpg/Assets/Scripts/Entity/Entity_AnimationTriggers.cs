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
    {//��Ҫ�˽⵱ǰ״̬��֪ͨ��ǰ״̬�˳�
        entity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }

}
