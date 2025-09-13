using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BasicAttactState : PlayerState
{
    private float attackVelocityTimer;

    private const int FirstComboIndex = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private int attackDir;

    private float lastTimeAttacked;
    private bool comboAttackQueued;

    public Player_BasicAttactState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("连击段数与连击位移数组匹配");
           comboLimit=player.attackVelocity.Length;
        }
    }
    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false; 
        ResetComboIndexIfNeeded();
        SyncAttackSpeed();

        attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;

            anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }



    public override void Update()
    {
        base.Update();
        HandlenAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        if (triggerCalled)
        {
            HandleStateExit();
        }
    }

    public override void Exit()
    {
         base.Exit();
        comboIndex++;
        lastTimeAttacked= Time.time;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
        {
            comboAttackQueued= true;
        }
    }

    private void HandlenAttackVelocity()
    {
        attackVelocityTimer-=Time.deltaTime;

        if (attackVelocityTimer<0)
        {
            player.SetVelocity(0, rb.velocity.y);
        }
        

    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex-1];

        attackVelocityTimer=player.attackVelocityDurantion;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);

    }

    private void ResetComboIndexIfNeeded()
    {
        if(Time.time > lastTimeAttacked + player.comboResetTime)
        {
            comboIndex=FirstComboIndex;
        }
        if (comboIndex > comboLimit)
        {
            comboIndex = FirstComboIndex;
        }
    }
}
