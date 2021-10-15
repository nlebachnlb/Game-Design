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

        var stateBehaviours = playerView.animator.GetBehaviours<StateMachineBehaviourExt>();
        foreach (var state in stateBehaviours)
        {
            state.InitController(this);
        }
    }

    public void Move()
    {
        if (playerModel.IsDashing) return;
        float moveBy = horizontalInputDirection * playerModel.moveSpeed * Time.fixedDeltaTime;
        playerView.rb.velocity = new Vector2(moveBy, playerView.rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKey(playerModel.jumpKey) && playerModel.isJumping == true)
        {
            if (playerModel.jumpTimeCounter > 0)
            {
                playerView.rb.velocity = Vector2.up * playerModel.jumpForce + new Vector2(playerView.rb.velocity.x, 0);
                playerModel.jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                playerModel.isJumping = false;
            }
        }

        if (Input.GetKeyUp(playerModel.jumpKey))
        {
            playerModel.isJumping = false;
        }
    }

    private void Dash()
    {
        if (playerModel.IsDashing)
        {
            switch (playerModel.dashPhase)
            {
                case 1:
                    {
                        if (playerModel.dashTimer > playerModel.maxDashTime * 0.1f)
                        {
                            playerView.rb.velocity = playerModel.dashDirection.normalized * playerModel.dashSpeed * Time.fixedDeltaTime;
                            playerModel.dashTimer -= Time.deltaTime;
                        }
                        else
                        {
                            // playerView.rb.velocity = Vector2.zero;
                            var vel = playerView.rb.velocity;
                            vel.y *= 0.5f;
                            vel.x *= 0.5f;
                            playerView.rb.velocity = vel;
                            playerView.rb.gravityScale = 5f;
                            playerModel.dashPhase = 2;
                            playerView.StopDash();
                        }
                    }
                    break;
                case 2:
                    {
                        var epsilon = playerModel.isWalking ? playerModel.moveSpeed * 0.8f * Time.fixedDeltaTime : playerModel.dashFriction * Time.fixedDeltaTime;
                        playerView.rb.velocity += -playerModel.dashDirection.normalized * playerModel.dashFriction * Time.fixedDeltaTime;

                        if (playerView.rb.velocity.magnitude <= epsilon || playerView.rb.velocity.normalized == -playerModel.dashDirection.normalized)
                        {
                            playerModel.dashPhase = 0;
                        }
                    }
                    break;
            }
        }
        else
        {
            if (playerModel.isGrounded && playerView.rb.velocity.y >= 0)
                playerModel.CurrentNumberofDash = 0;
        }

    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    private void Update()
    {
        playerModel.isGrounded = 
            Physics2D.OverlapCircle(playerModel.feetPosition.position, playerModel.checkRadius, playerModel.whatIsGround) &&
            playerView.rb.velocity.y <= 0f;

        if (playerModel.isGrounded && Input.GetKeyDown(playerModel.jumpKey))
        {
            // Jump down a platform
            if (verticalInputDicretion < 0)
            {
                playerView.collisionHandler.DisablePlatform();
            }
            else
            {
                playerModel.isJumping = true;
                playerModel.ResetJumpTimeCounter();
                playerView.rb.velocity = Vector2.up * playerModel.jumpForce + new Vector2(playerView.rb.velocity.x, 0);
            }
        }

        if (((Input.GetKeyDown(playerModel.dashKey)) || (Input.GetMouseButtonDown(1)))
            && (playerModel.CurrentNumberofDash < playerModel.MaxNumberOfDash))
        {
            playerModel.dashDirection =
                horizontalInputDirection != 0 || verticalInputDicretion != 0 ?
                new Vector2(horizontalInputDirection, verticalInputDicretion) :
                Vector2.right * playerModel.facing;

            playerModel.dashPhase = 1;
            playerModel.dashTimer = playerModel.maxDashTime;

            playerView.rb.gravityScale = 0f;

            playerModel.CurrentNumberofDash++;
            Debug.Log("INFO" + playerModel.isGrounded);
            Debug.Log("[INFO]" + playerModel.CurrentNumberofDash);

            playerView.PlayDash(playerModel.dashDirection);
        }
    }

    private void FixedUpdate()
    {
        UpdateLoop();
    }

    public void UpdateAnimator()
    {
        playerView.animator.SetBool("isOnGround", playerModel.isGrounded);
        playerView.animator.SetBool("isDashing", playerModel.IsDashing);
        playerView.animator.SetBool("isWalking", playerModel.isWalking);
        playerView.animator.SetFloat("yVelocity", playerView.rb.velocity.y);

        if (horizontalInputDirection != 0)
        {
            playerModel.facing = (int)Mathf.Sign(horizontalInputDirection);
            playerView.visual.flipX = playerModel.facing < 0;
        }
    }

    public void UpdateLoop()
    {
        horizontalInputDirection = Input.GetAxis("Horizontal");
        verticalInputDicretion = Input.GetAxisRaw("Vertical");
        playerModel.isWalking = horizontalInputDirection != 0;
        Move();
        Jump();
        Dash();
    }

    public void SpawnAt(Vector2 position, bool reborn = true)
    {
        playerView.transform.position = position;
    }

    public void ResetStat()
    {
        playerView.rb.velocity = Vector2.zero; 
    }

    #region Event callback
    public void OnDamaged(BaseHazard hazard)
    {
        app.controller.RespawnPlayer(app.model.lastSpawnPosition);
    }
    #endregion
}

