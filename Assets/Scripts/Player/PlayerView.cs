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

    // Start is called before the first frame update
    void Awake()
    {
        playerControler = app.controller.GetComponentInChildren<PlayerController>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        visual = GetComponent<SpriteRenderer>();
    }
}
