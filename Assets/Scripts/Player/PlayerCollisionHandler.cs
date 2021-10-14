using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class PlayerCollisionHandler : Controller<GamePlayApplication>
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            collision.gameObject.GetComponent<SpawnPoint>().OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            collision.gameObject.GetComponent<SpawnPoint>().OnPlayerExit();
        }
    }
}
