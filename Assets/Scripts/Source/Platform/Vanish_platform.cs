using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vanish_platform : MonoBehaviour
{
    public float recoverTime = 5f;
    public float waitTime = 1.5f;

    public bool isWaiting = false; 

    private Collider2D collider;
    private SpriteRenderer renderer;

    private Collider2D playerCollider; 

    private void Awake()
    {
        this.collider = GetComponent<Collider2D>();
        this.renderer = GetComponent<SpriteRenderer>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isWaiting)
        {
            if(collision.gameObject.transform.position.y >= this.collider.transform.position.y)
            {
                this.playerCollider = collision.gameObject.GetComponent<Collider2D>(); 
                StartCoroutine(Vanish()); 
            }
        }

    }

    private IEnumerator Vanish()
    {

        yield return new WaitForSeconds(this.waitTime);

        //Physics2D.IgnoreCollision(this.collider, playerCollider, true);
        collider.enabled = false;
        renderer.color = new Color(0, 0, 0, 0);
        this.isWaiting = true; 

        yield return new WaitForSeconds(this.recoverTime);

        //Physics2D.IgnoreCollision(this.collider, playerCollider, false);
        collider.enabled = true;
        renderer.color = new Color(0, 0, 255, 255);
        this.isWaiting = false; 
    }

}
