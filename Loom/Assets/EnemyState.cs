using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{

    // Guide:
    // When you create an enemyState named EnemytypeAnimState and create a constructor from this class
    // Make sure to add EnemyName _Enemy to the constructor, and also an empty variable EnemyName enemy to fill

    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; 
    }

   public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName, true);
        Debug.Log("Entering state " + animBoolName);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }
}