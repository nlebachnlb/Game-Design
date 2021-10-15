using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : BaseTrigger
{
    public string targetSceneName;

    public override void OnPlayerEnter(PlayerController player)
    {
        base.OnPlayerEnter(player);
        app.controller.SwitchScene(targetSceneName);
    }
}
