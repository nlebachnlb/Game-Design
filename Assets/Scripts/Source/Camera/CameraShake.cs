using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private Camera cameraView;

    public void Shake(Vector2 direction, float duration, float level = 0.5f)
    {
        cameraView.transform.DOShakePosition(duration, direction.normalized * level, 50, 0);
    }

    private void Awake()
    {
        cameraView = GetComponentInChildren<Camera>();
    }
}
