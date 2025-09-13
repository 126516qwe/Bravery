using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DashState : PlayerState
{
    private float dashDir;
    private float originalGravityScale;
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.dash.OnStarEffect();
        player.vfx.DoImageEchoEffect(player.dashDuration);

        dashDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;
        stateTimer = player.dashDuration;

        originalGravityScale =rb.gravityScale;
        rb.gravityScale = 0;
        
        player.health.SetCanTakeDamage(false);
        player.gameObject.layer = LayerMask.NameToLayer("Untargetable");
    }

    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.dashSpeed *dashDir, 0);
        if (stateTimer < 0f)
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            } else
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
    }
    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.health.SetCanTakeDamage(true);
        player.SetVelocity(0,0);    
        rb.gravityScale = originalGravityScale;


        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);

            }
            else 
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }
    }
}
