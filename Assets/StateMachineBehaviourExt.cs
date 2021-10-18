using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBehaviourExt : StateMachineBehaviour
{
    protected PlayerModel model;
    protected PlayerView view;
    protected PlayerController controller;

    public void InitController(PlayerController playerController)
    {
        controller = playerController;
        model = controller.PlayerModel;
        view = controller.PlayerView;
    }
}
