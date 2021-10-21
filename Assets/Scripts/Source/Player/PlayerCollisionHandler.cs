using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class PlayerCollisionHandler : Controller<GameplayApplication>
{
    public PlayerController playerController;

    private GameObject currentGhostPlatform;
    private GameObject currentVanishPlatform; 
    private Collider2D playerCollider;

    public void DisablePlatform()
    { 
        if (currentGhostPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    public void VanishPlatformCollision()
    {
        if (currentVanishPlatform != null)
        {
            if(playerCollider.transform.position.y >= currentVanishPlatform.transform.position.y)
            StartCoroutine(VanishCoroutine());
        }
    }

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GhostPlatform"))
        {
            currentGhostPlatform = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("VanishPlatform"))
        {
            currentVanishPlatform = collision.gameObject;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GhostPlatform"))
        {
            currentGhostPlatform = null;
        }

        if (collision.gameObject.CompareTag("VanishPlatform"))
        {
            currentVanishPlatform = null; 
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentGhostPlatform.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private IEnumerator VanishCoroutine()
    {
        Collider2D platformCollider = currentVanishPlatform.GetComponent<Collider2D>();
        Vanish_platform script = currentVanishPlatform.GetComponent<Vanish_platform>(); 
        SpriteRenderer platformRenderer = currentVanishPlatform.GetComponent<SpriteRenderer>(); 
        Debug.Log("start"); 
        yield return new WaitForSeconds(script.waitTime); 
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        platformRenderer.color = new Color(0, 0, 0, 0); 
        yield return new WaitForSeconds(script.recoverTime);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        platformRenderer.color = new Color(0, 0, 255, 255);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var trigger = collision.gameObject.GetComponent<BaseTrigger>();
        if (trigger != null)
        {
            trigger.OnPlayerEnter(playerController);
        }

        if (collision.gameObject.CompareTag("Collectible"))
        {
            var collectible = collision.gameObject.GetComponent<BaseCollectible>();
            collectible.OnCollected(playerController);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var trigger = collision.gameObject.GetComponent<BaseTrigger>();
        if (trigger != null)
        {
            trigger.OnPlayerExit(playerController);
        }
    }
}
