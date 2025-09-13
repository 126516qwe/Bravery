using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_fallState : Player_AirState
{
    public Player_fallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.wallDetected)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

    }//检测是否在地上，是的话准备切换状态
   
}