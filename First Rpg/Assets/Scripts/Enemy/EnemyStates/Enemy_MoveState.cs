using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.groundDetected == false || enemy.wallDetected == true)
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.GetMoveSpeed() * enemy.facingDir, rb.velocity.y);
        if (enemy.groundDetected == false || enemy.wallDetected)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
            
    }
}
