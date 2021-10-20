using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.ServiceLocator;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transitioner : Service
{
    [SerializeField]
    private Image mask;

    public void SwitchScene(string targetSceneName, float duration)
    {
        StartCoroutine(PlayTransition(targetSceneName, duration, Color.white));
    }

    private IEnumerator PlayTransition(string targetSceneName, float duration, Color color)
    {
        var startColor = color;
        startColor.a = 0f;
        //mask.color = startColor;
        mask.CrossFadeAlpha(1f, duration * 0.5f, true);
        yield return new WaitForSeconds(duration * 0.5f);
        yield return SceneManager.LoadSceneAsync(targetSceneName);
        yield return new WaitForSeconds(duration * 0.5f);
        mask.CrossFadeAlpha(0f, duration * 0.5f, true);
    }
}
