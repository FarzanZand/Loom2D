using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{

    // BattleState means enemy is in combat mode, going towards player to attack
    // You enter this state via enemyGroundedState-script when player in range
    // When battleState starts, you get a timer which shows how long you are in the battle and when to exit
    // When the player is within attackDistance, enemyAttackState starts

    [SerializeField] private Transform player; 
    private EnemySkeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy; 
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Check if player is within attackDistance and set time of battle-start for cooldown to end battle
        // If CanAttack cooldown is up, go attack
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {   // If player is too far, or out of combat too long, go back to idle
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
                stateMachine.ChangeState(enemy.idleState); 
        }


        // Check if player is to the right or left of enemy
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if(player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }


    // When you exit AttackState, lastTimeAttacked sets time. 
    // If time passes that time+cooldown, event triggers.
    private bool CanAttack() 
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true; 
        }

        return false; 
    }
}
