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

    private float dashBufferCounter = 0f;
    private int dashBufferInputs = 0;

    private float speed;

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
        LimitFallSpeed();
    }

    public void LimitFallSpeed()
    {
        if (PlayerModel.IsDashing)
            return;

        var vel = PlayerView.RB.velocity;
        if (vel.y < -Mathf.Abs(PlayerModel.fallSpeed))
        {
            vel.y = -Mathf.Abs(PlayerModel.fallSpeed);
        }
        PlayerView.RB.velocity = vel;
    }

    public void SpawnAt(Vector2 position, bool reborn = true)
    {
        PlayerView.transform.position = position;
    }

    public void ResetStat()
    {
        PlayerModel.ResetStats();
        PlayerView.RB.velocity = Vector2.zero;
        PlayerView.StopDash();
    }

    public void ActivateDashSkill(bool active)
    {
        PlayerModel.dashSkill = active;
    }

    #region Event callback
    public void OnDamaged(BaseHazard hazard)
    {
        if (PlayerModel.isDead) return;
        PlayerModel.isDead = true;
        app.controller.RespawnPlayer(app.model.lastSpawnPosition);
    }
    #endregion

    private void Awake()
    {
        Debug.Log(app); 
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
        if (PlayerModel.IsDashing)
        {
            //PlayerView.StopTails();
            return;
        }

        float moveBy = HorizontalInputDirection * PlayerModel.moveSpeed * Time.fixedDeltaTime;
        PlayerView.RB.velocity = new Vector2(moveBy, PlayerView.RB.velocity.y);
        PlayerView.UpdateIdleTail();

        if (HorizontalInputDirection != 0f)
            PlayerView.TailsFX.SetTailsLength(4, 4);
        else
            PlayerView.TailsFX.SetTailsLength();
    }

    private void Jump()
    {
        if (GetJumpKey() && PlayerModel.isJumping == true)
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

        if (GetJumpKeyUp())
        {
            PlayerModel.isJumping = false;
        }
    }

    private void Dash()
    {
        DashBufferProcess();

        if (PlayerModel.IsDashing)
        {
            switch (PlayerModel.dashPhase)
            {
                case 1:
                    {
                        if (speed < PlayerModel.dashSpeed)
                        {
                            speed += PlayerModel.dashAcceleration;
                        }
                        else
                        {
                            speed = PlayerModel.dashSpeed;
                            PlayerModel.dashPhase = 2;
                            var dir = PlayerModel.dashDirection.normalized;
                            var shakeDir = new Vector2(dir.y, dir.x);
                            app.view.CameraShakeFX.Shake(shakeDir, 0.3f);
                        }
                    }
                    break;
                case 2:
                    {
                        if (PlayerModel.dashTimer > PlayerModel.maxDashTime * 0.1f)
                        {
                            PlayerModel.dashTimer -= Time.deltaTime;
                        }
                        else
                        {
                            speed *= 0.5f;
                            PlayerModel.dashPhase = 3;
                            PlayerView.StopDash();
                        }
                    }
                    break;
                case 3:
                    {
                        var epsilon = PlayerModel.isWalking ? PlayerModel.moveSpeed * 0.8f * Time.fixedDeltaTime : PlayerModel.dashFriction * Time.fixedDeltaTime;
                        speed -= PlayerModel.dashFriction;

                        if (speed <= epsilon)
                        {
                            PlayerModel.dashPhase = 0;
                            PlayerView.RB.gravityScale = 5f;
                        }
                    }
                    break;
            }

            PlayerView.RB.velocity = PlayerModel.dashDirection.normalized * speed * Time.fixedDeltaTime;
        }
        else
        {
            if (PlayerModel.isGrounded && PlayerView.RB.velocity.y >= 0)
            {
                PlayerView.CollisionHandler.SwitchState(PlayerHitBoxState.Idle);
                PlayerModel.ResetDash();
            }
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

        if (PlayerModel.isGrounded)
        {
            PlayerModel.coyoteTimeCounter = PlayerModel.coyoteTime;
        }
        else
        {
            PlayerModel.coyoteTimeCounter -= Time.deltaTime;
        }

        JumpInputCheck();
        DashInputCheck();
    }

    private void FixedUpdate()
    {
        UpdateLoop();
    }

    private bool GetJumpKeyDown()
    {
        return
            Input.GetKeyDown(PlayerModel.jumpKey) ||
            Input.GetKeyDown(PlayerModel.jumpKeyAlt);
    }

    private bool GetJumpKeyUp()
    {
        return
            Input.GetKeyUp(PlayerModel.jumpKey) ||
            Input.GetKeyUp(PlayerModel.jumpKeyAlt);
    }

    private bool GetJumpKey()
    {
        return
            Input.GetKey(PlayerModel.jumpKey) ||
            Input.GetKey(PlayerModel.jumpKeyAlt);
    }

    private void JumpInputCheck()
    {
        if (PlayerModel.coyoteTimeCounter > 0f && GetJumpKeyDown() && PlayerModel.isJumping == false)
        {
            PlayerModel.coyoteTimeCounter = 0f;
            // Jump down a platform
            if (VerticalInputDirection < 0)
            {
                //PlayerView.CollisionHandler.DisablePlatform();
            }
            else
            {
                PlayerModel.isJumping = true;
                PlayerModel.ResetJumpTimeCounter();
                PlayerView.RB.velocity = Vector2.up * PlayerModel.jumpForce + new Vector2(PlayerView.RB.velocity.x, 0);
            }
        }
    }

    private void DashBufferProcess()
    {
        if (dashBufferCounter > 0f && (PlayerModel.CurrentNumberofDash < PlayerModel.MaxNumberOfDash)
            && (PlayerModel.dashPhase == 0) && dashBufferInputs > 0)
        {
            var rawHorInp = Input.GetAxisRaw("Horizontal");

            PlayerModel.dashDirection =
                rawHorInp != 0 || VerticalInputDirection != 0 ?
                new Vector2(rawHorInp, VerticalInputDirection) :
                Vector2.right * PlayerModel.facing;

            PlayerModel.dashPhase = 1;
            PlayerModel.dashTimer = PlayerModel.maxDashTime;

            PlayerView.RB.gravityScale = 0f;
            speed = 0f;

            PlayerModel.CurrentNumberofDash++;
            PlayerView.PlayDash(PlayerModel.dashDirection);

            if (PlayerModel.dashDirection.x != 0)
                PlayerView.CollisionHandler.SwitchState(PlayerHitBoxState.Dash);

            dashBufferInputs--;
        }
        else
        {
            if (dashBufferCounter >= 0f)
            {
                dashBufferCounter -= Time.deltaTime;
                if (dashBufferCounter < 0f)
                {
                    dashBufferInputs = 0;
                    dashBufferCounter = 0;
                }
            }
        }
    }

    private void DashInputCheck()
    {
        if (!PlayerModel.dashSkill)
            return;

        if ((Input.GetKeyDown(PlayerModel.dashKey)) || (Input.GetMouseButtonDown(1)) 
            && dashBufferInputs < PlayerModel.dashBufferSize
            && (PlayerModel.dashPhase == 3 || PlayerModel.dashPhase == 0))
        {
            dashBufferCounter = PlayerModel.dashBufferTime;
            dashBufferInputs++;
        }
    }
}

