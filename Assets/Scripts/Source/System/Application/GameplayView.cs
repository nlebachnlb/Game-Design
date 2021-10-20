using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayView : View<GameplayApplication>
{
    public CameraShake CameraShakeFX { get; private set; }
    public CameraFollow CameraFollow { get; private set; }

    [SerializeField]
    private GameObject cameraRig;

    private void Awake()
    {
        CameraShakeFX = cameraRig.GetComponent<CameraShake>();
        CameraFollow = cameraRig.GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
