using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AMVC;

public class BaseCollectible : View<GameplayApplication>
{
    public virtual void OnCollected(PlayerController player)
    {
        Destroy(gameObject);
    }
}
