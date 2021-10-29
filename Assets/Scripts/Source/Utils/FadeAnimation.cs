using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeAnimation : MonoBehaviour
{
    public float fadeInTime = 1f;
    public float fadeOutTime = 0.5f;
    private CanvasGroup group;

    public void FadeIn()
    {
        group.DOFade(1, fadeInTime);
    }

    public void FadeOut()
    {
        group.DOFade(0, fadeOutTime);
    }

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
}
