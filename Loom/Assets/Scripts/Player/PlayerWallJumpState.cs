using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{

    // 1. If you are in WallSlideState.cs and press jump, you enter this state
    // 2. Entering this state adds jumpForce to player, doing a jump opposite direction of where you are facing
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .4f;
        player.SetVelocity(5 * -player.facingDir, player.jumpForce); // Jump opposite direction of wall
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(player.airState);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
