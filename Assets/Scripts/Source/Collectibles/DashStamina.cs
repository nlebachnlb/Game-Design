using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStamina : BaseCollectible
{
    public float recoverTime;

    private Collider2D col;

    public override void OnCollected(PlayerController player)
    {
        player.PlayerModel.CurrentNumberofDash = 0;
        StartCoroutine(Recover());
    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private IEnumerator Recover()
    {
        col.enabled = false;
        // Temporary code
        GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(recoverTime);

        col.enabled = true;
        // Temporary code
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
