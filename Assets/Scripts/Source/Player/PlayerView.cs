using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : View<GamePlayApplication>
{
    private PlayerController playerControler;

    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Visual { get; private set; }
    public PlayerCollisionHandler CollisionHandler { get; private set; }

    [SerializeField]
    private GameObject feetPos;
    [SerializeField]
    private ParticleSystem dashFx, dashFx2;
    [SerializeField]
    private SpriteRenderer dashMotion;

    // Start is called before the first frame update
    void Awake()
    {
        playerControler = app.controller.GetComponentInChildren<PlayerController>();

        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Visual = GetComponent<SpriteRenderer>();
        CollisionHandler = GetComponent<PlayerCollisionHandler>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)feetPos.transform.position, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)this.transform.position, 3f);
    }

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
    }

    public void StopDash()
    {
        dashFx.Stop();
        Visual.enabled = true;
        dashMotion.gameObject.SetActive(false);
    }
}
