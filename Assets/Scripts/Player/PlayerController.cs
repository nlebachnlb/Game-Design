using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Controller<GamePlayApplication>
{
    public PlayerModel playerModel { get; private set; }
    public PlayerView playerView { get; private set; }
    public float horizontalInputDirection { get; private set; }
    public float verticalInputDicretion { get; private set; }

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
        if (playerModel.isDashing) return;

        float moveBy = horizontalInputDirection * playerModel.moveSpeed * Time.fixedDeltaTime;

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

    private void Dash()
    {
        if (Input.GetKeyDown(playerModel.dashKey))
        {
            playerModel.dashDirection =
                horizontalInputDirection != 0 || verticalInputDicretion != 0 ?
                new Vector2(horizontalInputDirection, verticalInputDicretion) :
                Vector2.right * playerModel.facing;

            playerModel.isDashing = true;
            playerModel.dashTimer = playerModel.maxDashTime;

            playerView.rb.gravityScale = 0;
        }

        if(playerModel.isDashing )
        {
            if (playerModel.dashTimer >= 0)
            {
                playerView.rb.velocity = Vector2.zero;
                playerView.rb.velocity = playerModel.dashDirection.normalized * playerModel.dashSpeed * Time.fixedDeltaTime;

                playerModel.dashTimer -= Time.deltaTime; 
            }
            else
            {
                playerView.rb.gravityScale = 5f; 

                playerModel.isDashing = false; 
            }
        }
        else 
        {

        }
        //if (playerModel.dashDirection == 0)
        //{
        //    if (horizontalInputDirection > 0)
        //        playerModel.dashDirection = 1;
        //    if (horizontalInputDirection < 0)
        //        playerModel.dashDirection = 2;
        //    if (verticalInputDicretion > 0)
        //        playerModel.dashDirection = 3;
        //    if (verticalInputDicretion < 0)
        //        playerModel.dashDirection = 4;
        //}
        //else
        //{
        //    if (playerModel.dashTime <= 0)
        //    {
        //        playerModel.dashDirection = 0;
        //        playerModel.dashTime = playerModel.startDashTime;
        //        playerView.rb.velocity = Vector2.zero; 
        //    }
        //    else
        //    {
        //        playerModel.dashTime -= Time.deltaTime;

        //        switch (playerModel.dashDirection)
        //        {
        //            case 1:
        //                playerView.rb.velocity = Vector2.right * playerModel.dashSpeed; 
        //                break;
        //            case 2:
        //                playerView.rb.velocity = Vector2.left * playerModel.dashSpeed;
        //                break;
        //            case 3:
        //                playerView.rb.velocity = Vector2.up * playerModel.dashSpeed;
        //                break;
        //            case 4:
        //                playerView.rb.velocity = Vector2.down * playerModel.dashSpeed;
        //                break;
        //        }
        //    }
        //}

    }

    private void Update()
    {
        UpdateLoop();
        UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        playerView.animator.SetBool("isOnGround", playerModel.isGrounded);
        playerView.animator.SetBool("isWalking", horizontalInputDirection != 0);
        playerView.animator.SetFloat("yVelocity", playerView.rb.velocity.y);

        if (horizontalInputDirection != 0)
        {
            playerModel.facing = (int)Mathf.Sign(horizontalInputDirection);
            playerView.visual.flipX = playerModel.facing < 0;
        }
    }

    public void UpdateLoop()
    {
        horizontalInputDirection = Input.GetAxisRaw("Horizontal");
        verticalInputDicretion = Input.GetAxisRaw("Vertical");
        Move(); 
        Jump();
        Dash(); 
    }
}
