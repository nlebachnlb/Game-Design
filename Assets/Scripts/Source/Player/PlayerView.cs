using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerView : View<GameplayApplication>
{
    private PlayerController playerControler;

    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Visual { get; private set; }
    public PlayerCollisionHandler CollisionHandler { get; private set; }
    public PlayerTailsFX TailsFX { get { return tailsFx; } }

    [SerializeField]
    private GameObject feetPos;
    [SerializeField]
    private ParticleSystem dashFx, tailLeft;
    [SerializeField]
    private SpriteRenderer dashMotion;
    [SerializeField]
    private PlayerTailsFX tailsFx;

    public void PlayDash(Vector2 direction)
    {
        dashFx.Play();
        dashMotion.gameObject.SetActive(true);
        dashMotion.gameObject.GetComponent<Animator>().SetTrigger("Dash");
        Visual.enabled = false;
        var angle = dashMotion.gameObject.transform.eulerAngles;
        var ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle.z = ang;
        dashMotion.transform.eulerAngles = angle;

        //tailLeft.transform.DORotate(angle, 0.5f).SetEase(Ease.InOutCirc);
        tailLeft.transform.eulerAngles = angle;
        tailsFx.SetTailsLength(8, 12);
    }

    public void StopDash()
    {
        dashFx.Stop();
        Visual.enabled = true;
        dashMotion.gameObject.SetActive(false);

        tailsFx.SetTailsLength();
    }

    public void PlayTail()
    {
        tailLeft.Play();
    }

    public void UpdateIdleTail()
    {
        if (playerControler.PlayerModel.facing == 1)
        {
            var targetVec = Vector3.forward;
            if (tailLeft.transform.eulerAngles.z > 90f)
                tailLeft.transform.eulerAngles = targetVec;

            if (playerControler.PlayerModel.isJumping)
                targetVec *= 25f;

            tailLeft.transform.eulerAngles = Vector3.Lerp(tailLeft.transform.eulerAngles, targetVec, Time.deltaTime * 5f);
        }
        else if (playerControler.PlayerModel.facing == -1)
        {
            var targetVec = Vector3.forward;
            if (tailLeft.transform.eulerAngles.z < 90f)
                tailLeft.transform.eulerAngles = targetVec * 180f;

            if (playerControler.PlayerModel.isJumping)
                targetVec *= 180f - 25f;
            else
                targetVec *= 180f;

            tailLeft.transform.eulerAngles = Vector3.Lerp(tailLeft.transform.eulerAngles, targetVec, Time.deltaTime * 5f);
        }

        //tailLeft.transform.eulerAngles = 
        //    Vector3.forward * 
        //    ((playerControler.PlayerModel.facing == 1 ? 0f : 180f) + 
        //    (playerControler.PlayerModel.isJumping ? 35f * playerControler.PlayerModel.facing : 0f));
    }
    
    public void StopTails()
    {
        tailLeft.Stop();
    }

    private void Awake()
    {
        playerControler = app.controller.GetComponentInChildren<PlayerController>();

        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Visual = GetComponent<SpriteRenderer>();
        CollisionHandler = GetComponent<PlayerCollisionHandler>();
    }

    private void Start()
    {
        StopTails();
        PlayTail();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)feetPos.transform.position, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)this.transform.position, 3f);
    }
}
