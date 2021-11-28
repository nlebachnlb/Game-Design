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
    public float dashAcceleration;
    public bool isDead;

    public float coyoteTime { get; private set; } = 0.13f;
    public float coyoteTimeCounter;

    public int MaxNumberOfDash;
    public int CurrentNumberofDash;

    [Space]
    [Header("Key")]
    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode jumpKey;
    public KeyCode jumpKeyAlt = KeyCode.X;
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
    public float fallSpeed = 20f;

    [Space]
    public float jumpTime;

    [Space]
    [Header("Dash Utils")]
    public float maxDashTime;

    public bool IsDashing { get { return dashPhase > 0; } }

    [Space]
    [Header("Skills")]
    public bool dashSkill;

    public void ResetJumpTimeCounter()
    {
        this.jumpTimeCounter = this.jumpTime;
    }

    public void ResetDash()
    {
        dashPhase = 0;
        CurrentNumberofDash = 0;
    }

    public void ResetStats()
    {
        ResetDash();
        isDead = false;
        dashTimer = maxDashTime;
    }

    private void Start()
    {
        playerControler = app.controller.GetComponentInChildren<PlayerController>();
        facing = 1;
    }
}
