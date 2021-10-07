using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    // Start is called before the first frame update
    public void Enter();

    public void HandleInput();

    public void LogicUpdate();

    public void PhysicsUpdate();

    public void Exit();
}
