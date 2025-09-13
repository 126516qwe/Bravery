using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    protected Transform player;
    protected Transform lastTarget;
    protected float lastTimeWasInBattle;
    protected float lastTimeAttacked = float.NegativeInfinity;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        if (player == null)
            player = enemy.GetPlayerReference();

        if (ShouldRetreat())
        {
            ShortRetreat();
        }
    }

    protected void ShortRetreat()
    {
        float x = (enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -DirectionToPlayer();
        float y = enemy.retreatVelocity.y;

        rb.velocity = new Vector2(x,y);
        enemy.HandleFlip(DirectionToPlayer());
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() == true)
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithinAttackRange() && enemy.PlayerDetected() &&CanAttack())
        {
            lastTimeAttacked = Time.time;
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            float xVelocity = enemy.canChasePlayer ? enemy.GetBattleMoveSpeed()  : 0.001f;
            enemy.SetVelocity(xVelocity * DirectionToPlayer(), rb.velocity.y);
        }
    }
    protected bool CanAttack() => Time.time > lastTimeAttacked + enemy.attackCooldown;
    protected void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;

        Transform newTarget = enemy.PlayerDetected().transform;

        if (newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }

    protected void UpdateBattleTimer()
    {
        lastTimeWasInBattle = Time.time;
    }

    protected bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battleTimeDuration;

    protected bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;



    protected bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    protected float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }

}
