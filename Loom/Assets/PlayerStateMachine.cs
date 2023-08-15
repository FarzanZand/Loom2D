using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player will be in different states. PlayerState is holding what the current state is of the player. PlayerStateMachine is backend controller of state changing.
// The PlayerState class instead is the one controlling the animator, and the player state itself, the logic for that specific state.
// There will be one class per state. State machine uses StateClass to change current state on PlayerState.



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
