using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the animation controller has a float blend which takes the velocity, and changes the anim depending on if the character is going up or down. 
// Input for the velocity is received via the PlayerState.cs script in its update. 

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
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState); 
    }
}
