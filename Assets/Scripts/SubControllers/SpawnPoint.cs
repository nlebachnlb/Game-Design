using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class SpawnPoint : View<GamePlayApplication>
{
    private int state;

    private void Start()
    {
        state = 0;
    }

    public void OnPlayerEnter()
    {
        app.model.lastSpawnPosition = transform.position;
        state = 1;
    }

    public void OnPlayerExit()
    {
        state = 0;
    }
}
