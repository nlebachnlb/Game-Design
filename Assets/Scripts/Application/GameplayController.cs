using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : Controller<GamePlayApplication>
{
    public PlayerController playerController { get; private set; }

    private void Awake()
    {
        playerController = GetComponentInChildren<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RespawnPlayer(app.model.lastSpawnPosition);
        }
    }

    public void RespawnPlayer(Vector2 position)
    {
        playerController.SpawnAt(position);
    }
}
