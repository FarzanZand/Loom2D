using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This controls the animator. 
// Note. Can we bake "PlayerState.cs into PlayerStateMachine.cs? Remove one class. 
public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb; 

    protected float xInput;
    private string animBoolName;

    protected float stateTimer;

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
    } 

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // Set timer for how long it should be in a state, different per state. 
        xInput = Input.GetAxisRaw("Horizontal");
        player.anim.SetFloat("yVelocity", rb.velocity.y); // Controls jump/fall 
        
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

}
