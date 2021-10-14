using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Boundary
{
    public Vector2 min, max;
}

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public Vector2 offset;

    [Range(0, 100)]
    public float cameraDistance;

    [Header("Camera Boundaries")]
    public List<Boundary> boundaries;
    public int currentBoundaryIndex;

    private void Start()
    {
        currentBoundaryIndex = 0;
    }

    private void FixedUpdate()
    {
        Vector2 desiredPosition = (Vector2)target.position + offset;
        Vector2 smoothedPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);
        var currBoundary = boundaries[currentBoundaryIndex];
        Vector3 finalPosition = 
            new Vector3
            (
                Mathf.Clamp(smoothedPosition.x, currBoundary.min.x, currBoundary.max.x),
                Mathf.Clamp(smoothedPosition.y, currBoundary.min.y, currBoundary.max.y),
                -cameraDistance
            );
        transform.position = finalPosition;
    }
}
