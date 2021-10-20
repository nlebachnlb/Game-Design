using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Controller<GameplayApplication>
{
    public PlayerModel PlayerModel { get; private set; }
    public PlayerView PlayerView { get; private set; }
    public float HorizontalInputDirection { get; private set; }
    public float VerticalInputDirection { get; private set; }
    public void UpdateAnimator()
    {
        PlayerView.Animator.SetBool("isOnGround", PlayerModel.isGrounded);
        PlayerView.Animator.SetBool("isDashing", PlayerModel.IsDashing);
        PlayerView.Animator.SetBool("isWalking", PlayerModel.isWalking);
        PlayerView.Animator.SetFloat("yVelocity", PlayerView.RB.velocity.y);

        if (HorizontalInputDirection != 0)
        {
            PlayerModel.facing = (int)Mathf.Sign(HorizontalInputDirection);
            PlayerView.Visual.flipX = PlayerModel.facing < 0;
        }
    }

    public void UpdateLoop()
    {
        HorizontalInputDirection = Input.GetAxis("Horizontal");
        VerticalInputDirection = Input.GetAxisRaw("Vertical");
        PlayerModel.isWalking = HorizontalInputDirection != 0;
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

    private void Awake()
    {
        PlayerModel = app.model.GetComponentInChildren<PlayerModel>();
        PlayerView = app.view.GetComponentInChildren<PlayerView>();
    }

    private void Start()
    {
        var stateBehaviours = PlayerView.Animator.GetBehaviours<StateMachineBehaviourExt>();
        foreach (var state in stateBehaviours)
        {
            state.InitController(this);
        }
    }

    private void Move()
    {
        if (PlayerModel.IsDashing) return;
        float moveBy = HorizontalInputDirection * PlayerModel.moveSpeed * Time.fixedDeltaTime;
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
        PlayerModel.isGrounded = 
            Physics2D.OverlapCircle(PlayerModel.feetPosition.position, PlayerModel.checkRadius, PlayerModel.whatIsGround) &&
            PlayerView.RB.velocity.y <= 0f;

        JumpInputCheck();
        DashInputCheck();
    }

    private void FixedUpdate()
    {
        UpdateLoop();
    }

    private void JumpInputCheck()
    {
        if (PlayerModel.isGrounded && Input.GetKeyDown(PlayerModel.jumpKey))
        {
            // Jump down a platform
            if (VerticalInputDirection < 0)
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
    }

    private void DashInputCheck()
    {
        if (((Input.GetKeyDown(PlayerModel.dashKey)) || (Input.GetMouseButtonDown(1)))
            && (PlayerModel.CurrentNumberofDash < PlayerModel.MaxNumberOfDash))
        {
            PlayerModel.dashDirection =
                HorizontalInputDirection != 0 || VerticalInputDirection != 0 ?
                new Vector2(Input.GetAxisRaw("Horizontal"), VerticalInputDirection) :
                Vector2.right * PlayerModel.facing;

            PlayerModel.dashPhase = 1;
            PlayerModel.dashTimer = PlayerModel.maxDashTime;

            PlayerView.RB.gravityScale = 0f;

            PlayerModel.CurrentNumberofDash++;
            PlayerView.PlayDash(PlayerModel.dashDirection);
        }
    }
}

