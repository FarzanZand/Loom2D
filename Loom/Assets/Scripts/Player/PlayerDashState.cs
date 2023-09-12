using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{

    // 1. CheckForDashInput() in Player.CS which always runs in Update().  
    // 2. If you press Key to dash and factors are favorable, change state to dashState
    // 3. Set dash duration, which ticks down in update. When it runs out, exit state. 
    // 4. Dash Player forward, animation plays via PlayerState Enter-method, which is same for all states as it is a parent
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.clone.CreateCloneOnDashStart();
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

        player.skill.clone.CreateCloneOnDashOver();
        player.SetZeroVelocity();
    }
}
