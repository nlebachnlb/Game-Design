using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class BaseTrigger : View<GamePlayApplication>
{
    public virtual void OnPlayerEnter(PlayerController player) { }
    public virtual void OnPlayerExit(PlayerController player) { }
}
