using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player will be in different states. PlayerState is holding what the current state is of the player. PlayerStateMachine is backend controller of state changing.
// There will be one class per state. State machine uses StateClass to change current state on PlayerState.

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; } // You can see the value, but not change it

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
