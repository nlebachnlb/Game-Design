using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneTransition
{
    None = 0,
    FadeThroughBlack = 1
}

public class GameplayController : Controller<GameplayApplication>
{
    public PlayerController playerController { get; private set; }

    public void RespawnPlayer(Vector2 position)
    {
        playerController.ResetStat();
        playerController.SpawnAt(position);
    }

    public void SwitchScene(string sceneName)
    {
        var transitioner = AppRoot.Instance.GetService<Transitioner>();
        transitioner.SwitchScene(sceneName, 1f);
    }

    private void Awake()
    {
        playerController = GetComponentInChildren<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RespawnPlayer(app.model.lastSpawnPosition);
        }
    }
}
