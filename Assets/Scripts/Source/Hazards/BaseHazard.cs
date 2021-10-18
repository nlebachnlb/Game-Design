using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class BaseHazard : BaseTrigger
{
    public override void OnPlayerEnter(PlayerController player)
    {
        player.OnDamaged(this);
    }
}
