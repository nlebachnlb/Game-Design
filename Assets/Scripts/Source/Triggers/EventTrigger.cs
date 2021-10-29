using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : BaseTrigger
{
    public UnityEvent enterActions, exitActions;

    public override void OnPlayerEnter(PlayerController player)
    {
        base.OnPlayerEnter(player);
        enterActions.Invoke();
    }

    public override void OnPlayerExit(PlayerController player)
    {
        base.OnPlayerExit(player);
        exitActions.Invoke();
    }
}
