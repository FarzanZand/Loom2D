using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Get here when jumping while grounded in PlayerGroundedState, or from walljump. 
// 2. Input for the velocity is received via the PlayerState.cs script in its update. JumpState switches to AirState when Velocity.y < 0. 
// Note: the animation controller has a float blend for AirState which takes the velocity, and changes the anim depending on if the character is going up or down. 

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0) // gives limited movement even while in the air. 
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState); 
    }
}
