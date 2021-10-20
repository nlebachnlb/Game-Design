using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : BaseTrigger
{
    public Transform lockPosition;
    public float changedSpeed = 0.05f;

    private Transform lastTarget;
    private float lastCamSpeed;

    public override void OnPlayerEnter(PlayerController player)
    {
        base.OnPlayerEnter(player);
        var cam = app.view.CameraFollow;
        lastCamSpeed = cam.smoothSpeed;
        cam.smoothSpeed = changedSpeed;
        lastTarget = cam.target;
        cam.target = lockPosition;
    }

    public override void OnPlayerExit(PlayerController player)
    {
    }
}
