using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class PlayerCollisionHandler : Controller<GamePlayApplication>
{
    public PlayerController playerController;

    private void Awake()
    {
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
