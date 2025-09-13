using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundState : PlayerState
{
    public Player_GroundState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0 && player.groundDetected  ==false )
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttactState);
        }

        if (input.Player.CounterAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (input.Player.RangeAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())
        {
            stateMachine.ChangeState(player.swordThrowState);
        }
    }
}
