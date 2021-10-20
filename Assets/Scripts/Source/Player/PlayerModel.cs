using System.Collections;
using System.Collections.Generic;
using Framework.AMVC;
using UnityEngine;

public class PlayerModel : Model<GameplayApplication>
{
    PlayerController playerControler;

    //[HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public int facing = 1;

    [HideInInspector]
    public float jumpTimeCounter;
    //[HideInInspector]
    public bool isJumping;
    public bool isWalking;

    public float dashTimer;
    public int dashPhase;
    public Vector2 dashDirection;
    public float dashFriction;

    public int MaxNumberOfDash;
    public int CurrentNumberofDash;

    [Space]
    [Header("Key")]
    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode jumpKey;
    public KeyCode downKey;
    public KeyCode upKey;
    public KeyCode dashKey;

    [Space]
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed;

    [Space]
    [Header("Jump Utils")]
    public float checkRadius;
    public Transform feetPosition;
    public LayerMask whatIsGround;

    [Space]
    public float jumpTime;

    [Space]
    [Header("Dash Utils")]
    public float maxDashTime;

    public bool IsDashing { get { return dashPhase > 0; } }

    public void ResetJumpTimeCounter()
    {
        this.jumpTimeCounter = this.jumpTime;
    }

    public void ResetDash()
    {
        CurrentNumberofDash = 0;
    }

    private void Start()
    {
        playerControler = app.controller.GetComponentInChildren<PlayerController>();
    }
}
