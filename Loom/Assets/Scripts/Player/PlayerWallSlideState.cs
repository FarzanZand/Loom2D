using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
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
