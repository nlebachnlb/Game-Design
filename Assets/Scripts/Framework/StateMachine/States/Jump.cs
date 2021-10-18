using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : State, IState
{
    public Jump(MonoBehaviour player) : base(player)
    {
        Debug.Log("[INFO] Jump state created!");

    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        
    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
    }
}
