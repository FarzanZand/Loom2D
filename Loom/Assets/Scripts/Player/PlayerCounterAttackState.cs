using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. If you press Q while grounded (see playerGroundedState), stateMachine changes to this PlayerCounterAttackState. 
// 2. You will be able to counter for the time set in stateTimer. 
// 3. While in this state, you will search for enemy colliders
// 4. If that enemy is in OpenCounterAttackWindow(), called in animator and canBeStunned, run event
// 5. Set SuccessfulCounterAttack as true, which will show that animation. End of anim runs event triggerCalled
// 6. triggerCalled, as it becomes true in Update(), state is changed back to PlayerIdleState. 


public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10; //Any value bigger than 1
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                }
            }
        }
        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
