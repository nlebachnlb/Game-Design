using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Framework.AMVC;

[System.Serializable]
public struct Boundary
{
    public Vector2 min, max;
}

public class CameraFollow : Controller<GameplayApplication>
{
    public enum CameraState
    {
        Normal = 0,
        Transition = 1
    }

    public Transform target;
    public float smoothSpeed;
    public float transitionDurationSeconds = 1f;
    public Vector2 offset;

    [Range(0, 100)]
    public float cameraDistance;

    [Header("Camera Boundaries")]
    public List<Boundary> boundaries;

    private int currentBoundaryIndex;
    private CameraState state;
    private Vector3 smoothedPosition;

    public void ChangeBoundary(int index)
    {
        if (index < 0 || index >= boundaries.Count)
        {
            Debug.LogError("[CameraFollow] Handled error: boundary not exist");
            return;
        }

        currentBoundaryIndex = index;
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        state = CameraState.Transition;

        Vector2 desiredPosition = (Vector2)target.position + offset;
        var currBoundary = boundaries[currentBoundaryIndex];
        Vector3 finalPosition =
            new Vector3
            (
                Mathf.Clamp(desiredPosition.x, currBoundary.min.x, currBoundary.max.x),
                Mathf.Clamp(desiredPosition.y, currBoundary.min.y, currBoundary.max.y),
                -cameraDistance
            );

        transform.DOMove(finalPosition, transitionDurationSeconds).SetEase(Ease.InOutSine);
        //app.controller.playerController.PlayerView.RB.simulated = false;
        //app.controller.playerController.enabled = false;

        yield return new WaitForSecondsRealtime(transitionDurationSeconds);

        //app.controller.playerController.enabled = true;
        //app.controller.playerController.PlayerView.RB.simulated = true;
        state = CameraState.Normal;
    }

    private void Start()
    {
        currentBoundaryIndex = 0;
        state = CameraState.Normal;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case CameraState.Normal:
                {
                    Vector2 desiredPosition = (Vector2)target.position + offset;
                    var currBoundary = boundaries[currentBoundaryIndex];
                    smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);

                    Vector3 finalPosition =
                        new Vector3
                        (
                            Mathf.Clamp(smoothedPosition.x, currBoundary.min.x, currBoundary.max.x),
                            Mathf.Clamp(smoothedPosition.y, currBoundary.min.y, currBoundary.max.y),
                            -cameraDistance
                        );

                    transform.position = finalPosition;
                }
                break;
        }
    }
}
