using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BlinkAnimation : MonoBehaviour
{
    public float blinkTime;

    private IEnumerator Start()
    {
        var image = GetComponent<Image>();

        while (true)
        {
            yield return new WaitForSeconds(blinkTime);
            image.enabled = !image.enabled;
        }
    }
}
