using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class SpawnPoint : BaseTrigger
{
    private int state;

    private void Start()
    {
        state = 0;
    }

    public override void OnPlayerEnter(PlayerController player)
    {
        app.model.lastSpawnPosition = transform.position;
        state = 1;
    }

    public override void OnPlayerExit(PlayerController player)
    {
        state = 0;
    }
}
