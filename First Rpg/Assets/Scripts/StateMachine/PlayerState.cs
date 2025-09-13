using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class PlayerState: EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player,StateMachine stateMachine,string animBoolName):base(stateMachine,animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
        entityStats = player.stats;
        skillManager = player.skillManager;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPerformedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }
        if (input.Player.UItimateSpell.WasPressedThisFrame() && skillManager.domainExpansion.CanUseSkill())
        {
            if (skillManager.domainExpansion.InstanDomain())
            {
                skillManager.domainExpansion.CreateDomain();

            }
            else
            {
                stateMachine.ChangeState(player.domainExpanslonState);
            }

            skillManager.domainExpansion.SetSkillOnCooldown();
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    

    private bool CanDash()
    {
        if(skillManager.dash.CanUseSkill()==false)
            return false;
        if (player.wallDetected)
        {
            return false;
        }
        if (stateMachine.currentState == player.dashState||stateMachine.currentState == player.domainExpanslonState)
        {
            return false;
        }

        return true;
    }
}
