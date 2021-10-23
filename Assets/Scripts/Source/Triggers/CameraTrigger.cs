using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : BaseTrigger
{
    public Transform lockPosition;
    public float changedSpeed = 0.05f;
    public int boundaryIndex;

    private Transform lastTarget;
    private float lastCamSpeed;
    private int state;

    [SerializeField]
    private bool oneWayTrigger, selfDisable, disableOnStart;

    [Header("Affected Triggers")]
    [SerializeField]
    private List<CameraTrigger> enabledTriggers, disabledTriggers;

    public override void OnPlayerEnter(PlayerController player)
    {
        base.OnPlayerEnter(player);
        switch (state)
        {
            case 0:
                {
                    var cam = app.view.CameraFollow;
                    cam.ChangeBoundary(boundaryIndex);
                    state = 1;

                    if (selfDisable)
                    {
                        gameObject.SetActive(false);
                    }

                    foreach(var trigger in enabledTriggers)
                    {
                        trigger.gameObject.SetActive(true);
                    }

                    foreach(var trigger in disabledTriggers)
                    {
                        trigger.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    public override void OnPlayerExit(PlayerController player)
    {
        if (oneWayTrigger)
        {
            Destroy(gameObject);
        }

        if (state != 0) state = 0;
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        state = 0;
        if (disableOnStart)
            gameObject.SetActive(false);
    }
}
