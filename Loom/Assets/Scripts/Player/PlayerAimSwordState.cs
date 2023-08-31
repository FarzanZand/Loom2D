using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    // 1. State is called when Mouse1 is clicked while grounded, holding it until you release.
    // 2. When you enter state, draws dots for path via swordThrow, from SwordSkill.cs 
    // 3. During state, when you release Mouse1, you go back to idle state. 

    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.swordThrow.DotsActive(true); 
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState); // Sets AimSwordState as false, which triggers ThrowSword animation clip


        // Flips the player towards side aimed, if you aim left or right
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)
            player.Flip();
        else if(player.transform.position.x < mousePosition.x && player.facingDir == -1)
            player.Flip();
    }
}
