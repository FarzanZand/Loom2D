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
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState); // Sets AimSwordState as false, which triggers ThrowSword animation clip
    }
}
