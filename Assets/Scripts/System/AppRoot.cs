using Framework.ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppRoot : ServiceLocator
{
    private IEnumerator Start()
    {
        Application.targetFrameRate = Config.SERVICE_DEFAULT_FPS;
        InitService();

        DontDestroyOnLoad(this.gameObject);

        yield return null;

        //SceneManager.LoadScene(firstSceneName);
        GetService<Transitioner>().SwitchScene(firstSceneName, 1f);
    }

    [SerializeField]
    private string firstSceneName;
}
