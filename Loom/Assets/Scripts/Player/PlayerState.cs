using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This controls the animator and holds the current state. See PlayerStateMachine.cs for more notes
// Every state has some shared things, added here, and some unique, in child objects
public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb; 

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player; 
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    } 

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // Set timer for how long it should be in a state, different per state. 
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y); // Controls jump/fall 
        
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }


    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
