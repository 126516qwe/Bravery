using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_jumpState : Player_AirState
{
    public Player_jumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();


        player.SetVelocity(rb.velocity.x,player.jumpForce);
    }//跳跃准备

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0&& stateMachine.currentState!=player.jumpAttackState)//保证下落攻击过程中不会自动变成下落状态
        {
            stateMachine.ChangeState(player.fallState);
        }

    }
    public override void Exit()
    {
        base.Exit();

    }
}
