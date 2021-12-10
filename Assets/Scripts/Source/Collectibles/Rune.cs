using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : BaseCollectible
{
    public bool isCollected;

    private Collider2D col;

    public GameObject position;

    public float smoothSpeed;

    public override void OnCollected(PlayerController player)
    {
        this.isCollected = true;
        col.enabled = false;
        this.transform.position = position.transform.position; 
        //GetComponent<SpriteRenderer>().enabled = false;  
    }

    private void FixedUpdate()
    {
    }

    private void Awake()
    {
        this.isCollected = false; 
        col = GetComponent<Collider2D>(); 
    }
}
