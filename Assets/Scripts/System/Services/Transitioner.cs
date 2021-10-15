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
        StartCoroutine(PlayTransition(duration, () =>
        {
            SceneManager.LoadScene(targetSceneName);
        }, Color.black));
    }

    private IEnumerator PlayTransition(float duration, System.Action onComplete, Color color)
    {
        var startColor = color;
        startColor.a = 0f;
        mask.color = startColor;
        mask.CrossFadeAlpha(1f, duration * 0.5f, true);
        yield return new WaitForSeconds(duration * 0.5f);
        onComplete?.Invoke();
        yield return new WaitForSeconds(duration * 0.5f);
        mask.CrossFadeAlpha(0f, duration * 0.5f, true);
    }
}
