using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SkeletonGroundedState : EnemyState
{
    // Parentclass for both IdleState and MoveState
    protected EnemySkeleton enemy;
    protected Transform player; 
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        // If detecting player in front, if if player is right behind, go battle mode
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }
}
