using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : View<GamePlayApplication>
{
    private PlayerController playerControler;

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public SpriteRenderer visual { get; private set; }

    [SerializeField]
    private GameObject feetPos;

    // Start is called before the first frame update
    void Awake()
    {
        playerControler = app.controller.GetComponentInChildren<PlayerController>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        visual = GetComponent<SpriteRenderer>();


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)feetPos.transform.position, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)this.transform.position, 3f);
    }
}
