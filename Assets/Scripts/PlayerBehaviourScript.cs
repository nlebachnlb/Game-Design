using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    private StateMachine CharacterFSM;

    private IState Stand;
    private IState Jump; 

    void Start()
    {
        CharacterFSM = new StateMachine();

        Stand = new Stand(this);
        Jump = new Jump(this);

        CharacterFSM.Initialize(Stand);

        Debug.Log("[INFO] Initialize character complete");
    }

    private void Update()
    {
        CharacterFSM.CurrentState.HandleInput();

        CharacterFSM.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        CharacterFSM.CurrentState.PhysicsUpdate(); 
    }
}
