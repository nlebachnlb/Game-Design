using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class SpawnPoint : BaseTrigger
{
    public override void OnPlayerEnter(PlayerController player)
    {
        app.model.lastSpawnPosition = transform.position;
    }

    public override void OnPlayerExit(PlayerController player)
    {
    }
}
