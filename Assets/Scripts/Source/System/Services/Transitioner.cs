using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.ServiceLocator;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transitioner : Service
{
    [SerializeField]
    private Image mask, glowMask;

    public void Glow(float duration)
    {
        glowMask.color = Color.red;
        glowMask.CrossFadeAlpha(0f, duration, true);
    }

    public void SwitchScene(string targetSceneName, float duration)
    {
        StartCoroutine(PlayTransition(targetSceneName, duration, Color.black));
    }

    public void SwitchCamera(Vector3 position, float transitionDuration, System.Action onComplete)
    {
        StartCoroutine(PlayTransition2(position, transitionDuration, Color.black, onComplete));
    }

    private IEnumerator PlayTransition(string targetSceneName, float duration, Color color)
    {
        var startColor = color;
        startColor.a = 0f;  
        mask.CrossFadeAlpha(1f, duration * 0.5f, true);
        yield return new WaitForSeconds(duration * 0.5f);
        yield return SceneManager.LoadSceneAsync(targetSceneName);
        yield return new WaitForSeconds(duration * 0.5f);
        mask.CrossFadeAlpha(0f, duration * 0.5f, true);
    }

    private IEnumerator PlayTransition2(Vector3 position, float duration, Color color, System.Action onComplete)
    {
        var startColor = color;
        startColor.a = 0f;
        mask.CrossFadeAlpha(1f, duration * 0.5f, true);
        yield return new WaitForSeconds(duration * 0.5f);
        onComplete();
        yield return new WaitForSeconds(duration * 0.5f);
        mask.CrossFadeAlpha(0f, duration * 0.5f, true);
    }
}
