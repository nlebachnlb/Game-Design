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
        //playerController.ResetStat();
        //playerController.SpawnAt(position);
        StartCoroutine(RespawnProcess(position));
    }

    public void SwitchScene(string sceneName)
    {
        var transitioner = AppRoot.Instance.GetService<Transitioner>();
        transitioner.SwitchScene(sceneName, 1f);
    }

    private IEnumerator RespawnProcess(Vector2 position)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        Instantiate(app.view.ExplosionFX, playerController.PlayerView.transform.position, Quaternion.identity);
        playerController.PlayerView.gameObject.SetActive(false);
        app.view.CameraShakeFX.Shake(new Vector2(1f, 1f), 1f);
        yield return new WaitForSecondsRealtime(0.5f);
        var transitioner = AppRoot.Instance.GetService<Transitioner>();
        var camPos = app.view.CameraFollow.transform.position;
        camPos.x = position.x;
        camPos.y = position.y;
        transitioner.SwitchCamera(camPos, 0.25f, () =>
        {
            app.view.CameraFollow.transform.position = camPos;
            playerController.PlayerView.gameObject.SetActive(true);
            playerController.ResetStat();
            playerController.SpawnAt(position);
        });

        yield return new WaitForSecondsRealtime(0.25f);
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
