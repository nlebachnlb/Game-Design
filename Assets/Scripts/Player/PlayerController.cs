using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Controller<GamePlayApplication>
{
    public PlayerModel playerModel { get; private set; }
    public PlayerView playerView { get; private set; }
    public float inputDirection { get; private set; }

    private void Start()
    {
        playerModel = app.model.GetComponentInChildren<PlayerModel>();
        playerView = app.view.GetComponentInChildren<PlayerView>();

        Debug.Log(playerView != null);
        Debug.Log(playerView.animator != null);

        var stateBehaviours = playerView.animator.GetBehaviours<StateMachineBehaviourExt>();
        foreach (var state in stateBehaviours)
        {
            state.InitController(this);
        }
    }

    public void Move()
    {
        float moveBy = inputDirection * playerModel.moveSpeed * Time.deltaTime;
        playerView.rb.velocity = new Vector2(moveBy, playerView.rb.velocity.y);
    }

    private void Jump()
    {
        playerModel.isGrounded = Physics2D.OverlapCircle(playerModel.feetPosition.position, playerModel.checkRadius, playerModel.whatIsGround);

        if (playerModel.isGrounded && Input.GetKeyDown(playerModel.jumpKey))
        {
            playerModel.isJumping = true;
            playerModel.ResetJumpTimeCounter(); 
            playerView.rb.velocity = Vector2.up * playerModel.jumpForce + new Vector2(playerView.rb.velocity.x, 0); 
        }

        if (Input.GetKey(playerModel.jumpKey) && playerModel.isJumping == true)
        {
            if(playerModel.jumpTimeCounter > 0)
            {
                playerView.rb.velocity = Vector2.up * playerModel.jumpForce + new Vector2(playerView.rb.velocity.x, 0);
                playerModel.jumpTimeCounter -= Time.deltaTime; 
            }
            else
            {
                playerModel.isJumping = false; 
            }
        }

        if(Input.GetKeyUp(playerModel.jumpKey))
        {
            playerModel.isJumping = false; 
        }
    }

    private void Update()
    {
        UpdateLoop();
        UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        playerView.animator.SetBool("isOnGround", playerModel.isGrounded);
        playerView.animator.SetBool("isWalking", inputDirection != 0);
        playerView.animator.SetFloat("yVelocity", playerView.rb.velocity.y);

        if (inputDirection != 0)
        {
            playerModel.facing = (int)Mathf.Sign(inputDirection);
            playerView.visual.flipX = playerModel.facing < 0;
        }
    }

    public void UpdateLoop()
    {
        inputDirection = Input.GetAxisRaw("Horizontal");
        Move();
        Jump(); 
    }
}
