using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{

    // 1. PlayerIdleState is the default state you get to when other state runs out or if you just stand still on ground.
    // 2. From this state, you can easily transition to many other states
    // 3. Parentclass is PlayerGroundedState, which itself is a child of PlayerState. idleState controls are in parent
    // 4. If player is not busy in player.isBusy, and you move player not into a wall, you change to MoveState.
    // Note: Perhaps add isBusy-functionality to groundedState controls, so you can block easily, perhaps when in menu for instance
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput == player.facingDir && player.IsWallDetected())
            return; // Stop the player from moving while running into wall. Resets MoveState to IdleState

        if(xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
