using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class PlayerCollisionHandler : Controller<GameplayApplication>
{
    public PlayerController playerController;

    [SerializeField]
    public Vector2 dashHitBoxOffset, dashHitBoxSize;
    [SerializeField]
    public Vector2 idleHitBoxOffset, idleHitBoxSize;

    private GameObject currentGhostPlatform;
    private BoxCollider2D playerCollider;

    public void DisablePlatform()
    {
        if (currentGhostPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    public void SwitchState(PlayerHitBoxState destState)
    {
        Vector2 targetOffset = Vector2.zero;
        Vector2 targetSize = Vector2.zero;

        switch (destState)
        {
            case PlayerHitBoxState.Idle:
                {
                    targetOffset = idleHitBoxOffset;
                    targetSize = idleHitBoxSize;
                }
                break;
            case PlayerHitBoxState.Dash:
                {
                    targetOffset = dashHitBoxOffset;
                    targetSize = dashHitBoxSize;
                }
                break;
        }

        if (targetSize != Vector2.zero)
        {
            playerCollider.offset = targetOffset;
            playerCollider.size = targetSize;
        }
    }

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GhostPlatform"))
        {
            currentGhostPlatform = collision.gameObject;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GhostPlatform"))
        {
            currentGhostPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentGhostPlatform.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
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
