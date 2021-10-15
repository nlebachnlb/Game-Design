using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class PlayerCollisionHandler : Controller<GamePlayApplication>
{
    public PlayerController playerController;

    private GameObject currentGhostPlatform;
    private Collider2D playerCollider;

    public void DisablePlatform()
    {
        if (currentGhostPlatform != null)
        {
            StartCoroutine(DisableCollision());
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
        if (collision.CompareTag("SpawnPoint"))
        {
            collision.gameObject.GetComponent<SpawnPoint>().OnPlayerEnter(playerController);
        }

        if (collision.CompareTag("Hazard"))
        {
            collision.gameObject.GetComponent<BaseHazard>().OnPlayerEnter(playerController);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            collision.gameObject.GetComponent<SpawnPoint>().OnPlayerExit(playerController);
        }

        if (collision.CompareTag("Hazard"))
        {
            collision.gameObject.GetComponent<BaseHazard>().OnPlayerExit(playerController);
        }
    }
}
