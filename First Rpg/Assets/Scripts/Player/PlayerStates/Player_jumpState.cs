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
    }//��Ծ׼��

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0&& stateMachine.currentState!=player.jumpAttackState)//��֤���乥�������в����Զ��������״̬
        {
            stateMachine.ChangeState(player.fallState);
        }

    }
    public override void Exit()
    {
        base.Exit();

    }
}
