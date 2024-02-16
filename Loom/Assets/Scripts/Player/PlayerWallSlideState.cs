using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{

    // 1. If you are in AirState and hit a wall via player.IsWallDetected(), change state to WallSlideState
    // 2. If you push away from wall, you fall off
    // 3. If you press down, you slide faster
    // 4. If you hit ground, you reset back to idleState. Ground checked in IsGroundDetected(), part of entity, parent of player
    // 5. If you press jump button while wall-sliding, swap to wallJumpState which jumps you and adds force on enter
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (player.IsWallDetected() == false)
            stateMachine.ChangeState(player.airState);

        base.Update();
        if (Input.GetKey(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return; // Wall jump
        }

        if(xInput != 0 && player.facingDir != xInput) // Check when pushing away from wall
            stateMachine.ChangeState(player.idleState);

        if(yInput < 0) // If pressing down, slide faster
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
        rb.velocity = new Vector2(0, rb.velocity.y * 0.7f); // slow descent while sliding

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }


}
