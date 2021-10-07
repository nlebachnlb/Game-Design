using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand :State, IState
{
    public Stand(MonoBehaviour player) : base(player)
    {
        Debug.Log("[INFO] Stand state created! ");
    }

    public override void Enter()
    {
        Debug.Log("[INFO] Currently in Stand state");
    }

    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        base.HandleInput();

    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
    }

}
