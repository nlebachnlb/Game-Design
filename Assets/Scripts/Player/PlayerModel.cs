using System.Collections;
using System.Collections.Generic;
using Framework.AMVC;
using UnityEngine;

public class PlayerModel : Model<GamePlayApplication>
{
    PlayerController playerControler;

    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public int facing = 1;

    [HideInInspector]
    public float jumpTimeCounter;
    [HideInInspector]
    public bool isJumping;

    public float dashTimer;
    public bool isDashing;
    public Vector2 dashDirection; 

    [Space]
    [Header("Key")]
    public KeyCode rightKey; 
    public KeyCode leftKey;
    public KeyCode jumpKey;
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



    // Start is called before the first frame update
    void Start()
    {
        playerControler = app.controller.GetComponentInChildren<PlayerController>(); 
    }

    public void ResetJumpTimeCounter()
    {
        this.jumpTimeCounter = this.jumpTime; 
    }
}
