using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Refund all skills", fileName = "Item effect data - Refund all skills")]
public class ItemEffect_RefundAllSkills : ItemEffect_DataSO
{   

    Skill_DataSO skill_DataSO;
    public override void ExecuteEffect()
    { 
        UI.instance.skillTreeUI.RefundAllSkills();

        foreach (var skill in Player.instance.skillManager.allSkills)
        {
           skill.ResetUpgrade();
        }
    }
}
