using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState CurrentState;

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(IState newState)
    {
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = newState;
        newState.Enter();
    }


}
