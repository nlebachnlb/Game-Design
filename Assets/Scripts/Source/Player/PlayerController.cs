using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Controller<GamePlayApplication>
{
    public PlayerModel PlayerModel { get; private set; }
    public PlayerView PlayerView { get; private set; }
    public float horizontalInputDirection { get; private set; }
    public float verticalInputDicretion { get; private set; }

    private void Start()
    {
        PlayerModel = app.model.GetComponentInChildren<PlayerModel>();
        PlayerView = app.view.GetComponentInChildren<PlayerView>();

        var stateBehaviours = PlayerView.Animator.GetBehaviours<StateMachineBehaviourExt>();
        foreach (var state in stateBehaviours)
        {
            state.InitController(this);
        }
    }

    public void Move()
    {
        if (PlayerModel.IsDashing) return;
        float moveBy = horizontalInputDirection * PlayerModel.moveSpeed * Time.fixedDeltaTime;
        PlayerView.RB.velocity = new Vector2(moveBy, PlayerView.RB.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKey(PlayerModel.jumpKey) && PlayerModel.isJumping == true)
        {
            if (PlayerModel.jumpTimeCounter > 0)
            {
                PlayerView.RB.velocity = Vector2.up * PlayerModel.jumpForce + new Vector2(PlayerView.RB.velocity.x, 0);
                PlayerModel.jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                PlayerModel.isJumping = false;
            }
        }

        if (Input.GetKeyUp(PlayerModel.jumpKey))
        {
            PlayerModel.isJumping = false;
        }
    }

    private void Dash()
    {
        if (PlayerModel.IsDashing)
        {
            switch (PlayerModel.dashPhase)
            {
                case 1:
                    {
                        if (PlayerModel.dashTimer > PlayerModel.maxDashTime * 0.1f)
                        {
                            PlayerView.RB.velocity = PlayerModel.dashDirection.normalized * PlayerModel.dashSpeed * Time.fixedDeltaTime;
                            PlayerModel.dashTimer -= Time.deltaTime;
                        }
                        else
                        {
                            // PlayerView.RB.velocity = Vector2.zero;
                            var vel = PlayerView.RB.velocity;
                            vel.y *= 0.5f;
                            vel.x *= 0.5f;
                            PlayerView.RB.velocity = vel;
                            PlayerView.RB.gravityScale = 5f;
                            PlayerModel.dashPhase = 2;
                            PlayerView.StopDash();
                        }
                    }
                    break;
                case 2:
                    {
                        var epsilon = PlayerModel.isWalking ? PlayerModel.moveSpeed * 0.8f * Time.fixedDeltaTime : PlayerModel.dashFriction * Time.fixedDeltaTime;
                        PlayerView.RB.velocity += -PlayerModel.dashDirection.normalized * PlayerModel.dashFriction * Time.fixedDeltaTime;

                        if (PlayerView.RB.velocity.magnitude <= epsilon || PlayerView.RB.velocity.normalized == -PlayerModel.dashDirection.normalized)
                        {
                            PlayerModel.dashPhase = 0;
                        }
                    }
                    break;
            }
        }
        else
        {
            if (PlayerModel.isGrounded && PlayerView.RB.velocity.y >= 0)
                PlayerModel.CurrentNumberofDash = 0;
        }

    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    private void Update()
    {


        if (PlayerModel.isGrounded && Input.GetKeyDown(PlayerModel.jumpKey))
        {
            // Jump down a platform
            if (verticalInputDicretion < 0)
            {
                PlayerView.CollisionHandler.DisablePlatform();
            }
            else
            {
                PlayerModel.isJumping = true;
                PlayerModel.ResetJumpTimeCounter();
                PlayerView.RB.velocity = Vector2.up * PlayerModel.jumpForce + new Vector2(PlayerView.RB.velocity.x, 0);
            }
        }

        if (((Input.GetKeyDown(PlayerModel.dashKey)) || (Input.GetMouseButtonDown(1)))
            && (PlayerModel.CurrentNumberofDash < PlayerModel.MaxNumberOfDash))
        {
            PlayerModel.dashDirection =
                horizontalInputDirection != 0 || verticalInputDicretion != 0 ?
                new Vector2(horizontalInputDirection, verticalInputDicretion) :
                Vector2.right * PlayerModel.facing;

            PlayerModel.dashPhase = 1;
            PlayerModel.dashTimer = PlayerModel.maxDashTime;

            PlayerView.RB.gravityScale = 0f;

            PlayerModel.CurrentNumberofDash++;
            Debug.Log("INFO" + PlayerModel.isGrounded);
            Debug.Log("[INFO]" + PlayerModel.CurrentNumberofDash);

            PlayerView.PlayDash(PlayerModel.dashDirection);
        }
    }

    private void FixedUpdate()
    {
        PlayerModel.isGrounded =
    Physics2D.OverlapCircle(PlayerModel.feetPosition.position, PlayerModel.checkRadius, PlayerModel.whatIsGround) &&
    PlayerView.RB.velocity.y <= 0f;
        UpdateLoop();
    }

    public void UpdateAnimator()
    {
        PlayerView.Animator.SetBool("isOnGround", PlayerModel.isGrounded);
        PlayerView.Animator.SetBool("isDashing", PlayerModel.IsDashing);
        PlayerView.Animator.SetBool("isWalking", PlayerModel.isWalking);
        PlayerView.Animator.SetFloat("yVelocity", PlayerView.RB.velocity.y);

        if (horizontalInputDirection != 0)
        {
            PlayerModel.facing = (int)Mathf.Sign(horizontalInputDirection);
            PlayerView.Visual.flipX = PlayerModel.facing < 0;
        }
    }

    public void UpdateLoop()
    {
        horizontalInputDirection = Input.GetAxis("Horizontal");
        verticalInputDicretion = Input.GetAxisRaw("Vertical");
        PlayerModel.isWalking = horizontalInputDirection != 0;
        Move();
        Jump();
        Dash();
    }

    public void SpawnAt(Vector2 position, bool reborn = true)
    {
        PlayerView.transform.position = position;
    }

    public void ResetStat()
    {
        PlayerView.RB.velocity = Vector2.zero; 
    }

    #region Event callback
    public void OnDamaged(BaseHazard hazard)
    {
        app.controller.RespawnPlayer(app.model.lastSpawnPosition);
    }
    #endregion
}

