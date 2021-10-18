using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : BaseTrigger
{
    public Transform lockPosition;

    private Transform lastTarget;

    public override void OnPlayerEnter(PlayerController player)
    {
        base.OnPlayerEnter(player);
        var cam = Camera.main.GetComponent<CameraFollow>();
        lastTarget = cam.target;
        cam.target = lockPosition;
    }

    public override void OnPlayerExit(PlayerController player)
    {
    }
}
