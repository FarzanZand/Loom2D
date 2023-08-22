using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2; 

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow ) // Only 2 attacks, but no attack outside of combowindow time
            comboCounter = 0;

        // if grounded, state PlayerGroundedState is active, and there you can press mouse, which triggers this attack stack
        // the comboCounter decides if it is attack 1 2 or 3 which is called via animator sub-state-machine
        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y); // Move forward a little with an attack

        stateTimer = 0.1f;

        // If you want  for instance a buff which increases attack speed, use. player.anim.speed = 1.2f; Close at exit to default.
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f); // Block player from moving between attacks

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity(); // Stand still when attacking


        // TriggerCalled becomes true when AnimationFinishTrigger() in PlayerState is called, in this case via animation trigger in animator
        // Calls AnimationTrigger function in player which calls AnimationFinishTrigger in PlayerState.
        // See PlayerAnimationsTriggers.cs for more details

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
