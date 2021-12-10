using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Vanish_platform : MonoBehaviour
{
    public float recoverTime = 5f;
    public float waitTime = 1.5f;

    public bool isWaiting = false; 

    private Collider2D collider;
    private SpriteRenderer renderer;

    private Collider2D playerCollider;

    [SerializeField]
    private ParticleSystem sandFx;
    [SerializeField]
    private GameObject visual;
    [SerializeField]
    private Animator visualAnim;

    private void Awake()
    {
        this.collider = GetComponent<Collider2D>();
        this.renderer = GetComponent<SpriteRenderer>(); 
    }

    private void Start()
    {
        var shape = sandFx.shape;
        shape.scale = transform.localScale;
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
        //visual.transform.DOShakePosition(this.waitTime, 0.3f);
        sandFx.Play();

        yield return new WaitForSeconds(this.waitTime);

        //Physics2D.IgnoreCollision(this.collider, playerCollider, true);
        collider.enabled = false;
        renderer.color = new Color(0, 0, 0, 0);
        this.isWaiting = true;
        visualAnim.SetTrigger("Vanish");
        sandFx.Stop();

        yield return new WaitForSeconds(this.recoverTime);

        visualAnim.SetTrigger("Appear");
        //Physics2D.IgnoreCollision(this.collider, playerCollider, false);
        collider.enabled = true;
        renderer.color = new Color(0, 0, 255, 255);
        this.isWaiting = false; 
    }

}
