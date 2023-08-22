using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration; // Set the dash timer
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundDetected() && player.IsWallDetected()) // If you dash into a wall while in air
                stateMachine.ChangeState(player.wallSlideState);

        player.SetVelocity(player.dashSpeed *player.dashDir, 0); // Start Dash

        if (stateTimer < 0) // End dashState and return to idle after timer
            stateMachine.ChangeState(player.idleState); 
    }

    public override void Exit()
    {
        base.Exit();
        player.ZeroVelocity();
    }
}