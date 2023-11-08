using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    // 1. if grounded, state PlayerGroundedState is active, and there you can press mouse, which triggers this attack stack
    // 2. Player PrimaryAttack is a comboattack with three states. All controlled here, but with 3 animationclips in a PrimaryAttack substatemachine
    // 3. Type of attack used depends on the comboCounter, ticks up after every attack when you exit this state for each anim-clip.
    // 4. When Combo is above max (in this case, chain is 3, so if comboCounter is > 2, you reset to 0, doing first attack
    // 5. The animator decides which attack depending on which ComboCounter-integer you send in to its PrimaryAttack substate. 
    // 6. Added some flare, you move forward a bit on an attack, but also blocks player from moving between attacks a time via "BusyFor". 
    // 7. In the animator clip, an event is at the end of the animation calling setting triggerCalled to true, reseting to idle, ready for next attack
    // 8. ComboCounter is also reset if you wait too long between attacks. 

    public int comboCounter { get; private set; }

    private float lastTimeAttacked; // Checks how long it was since you last attacked
    private float comboWindow = 2;  // Checks how long you can wait between attack chains before comboCounter is reset to 0. 

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // LastTimeAttacked saves time of attack. If that time + combowindow in secs time is later than current Time.time, reset.
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow ) 
            comboCounter = 0;

        // if grounded, state PlayerGroundedState is active, and there you can press mouse, which triggers this attack stack
        // the comboCounter decides if it is attack 1 2 or 3 which is called via animator sub-state-machine
        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;
        xInput = Input.GetAxisRaw("Horizontal"); // Needed to switch direction mid-combo attack sequence. 

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


        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
