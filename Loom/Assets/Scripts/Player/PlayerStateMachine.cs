using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Player.cs creates objects of all states and sets them
// 2. You by default enter idleState. 
// 3. You enter a new state via PlayerStateMachine stateMachine, which is in PlayerState
// 4. Running stateMachine.ChangeState(), you leave current state, set new state, and enter it. 
// 5. When you enter new state, which is a child of PlayerState, it runs enter() in both parent and child
// 6. Parent sets the animator bool, rb and resets triggerCalled to false.
// 7. Child sets things specific for that state. 

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; } 

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    // When you change a state, it starts by calling exit on former state, sets new state, than calls enter on new state. 
    // The playerXstate script calls the ChangeState function, and they all are childs of PlayerState. 
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
