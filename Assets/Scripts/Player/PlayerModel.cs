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
    public int facing;

    [HideInInspector]
    public float jumpTimeCounter;
    [HideInInspector]
    public bool isJumping;

    [Space]
    [Header("Key")]
    public KeyCode jumpKey;
    public KeyCode rightKey; 
    public KeyCode leftKey; 

    [Space]
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;

    [Space]
    [Header("Jump Utils")] 
    public float checkRadius;
    public Transform feetPosition;
    public LayerMask whatIsGround;

    [Space]
    public float jumpTime;


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
