using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MageQuickRetreatState : EnemyState
{
    private Enemy_Mage enemyMage;
    private Vector3 startPositon;
    private Transform player;

    public Enemy_MageQuickRetreatState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyMage = enemy as Enemy_Mage;    
    }

    public override void Enter()
    {
         base.Enter();

        if(player == null)
            player = enemy.GetPlayerReference();

        startPositon = enemy.transform.position;

        rb.velocity = new Vector2(enemyMage.retreatSpeed * -DirectionToPlayer(), 0);
        enemy.HandleFlip(DirectionToPlayer());

        enemy.gameObject.layer = LayerMask.NameToLayer("Untargetable");
        enemy.vfx.DoImageEchoEffect(.5f);
    }

    public override void Update()
    {
        base.Update();

        bool rechedMaxDistance = Vector2.Distance(enemy.transform.position, startPositon) > enemyMage.retreatMaxDistance;

        if (rechedMaxDistance|| enemyMage.CanMoveBackwards())
            stateMachine.ChangeState(enemyMage.mageSpellCastState);
        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.vfx.StopImageEchoEffect();
        enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");

    }

    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }

}
