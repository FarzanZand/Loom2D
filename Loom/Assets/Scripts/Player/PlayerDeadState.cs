using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    // 1. Entity dies when HP is less than 0, which is checked in CharacterStats.TakeDamage(). 
    // 2. When dead, TakeDamage() calls Die() in child, which calls Die() in player, activating deadState. 
    // 3. The reason we have this extra step, player and enemy will have be different, enemy being enemy.Die(). 

    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
    }
}
