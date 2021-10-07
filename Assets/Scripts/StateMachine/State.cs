using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected MonoBehaviour player;

    public State(MonoBehaviour player)
    {
        this.player = player;
    }

    public virtual void Enter()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
