using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayModel : Model<GameplayApplication>
{
    public PlayerModel PlayerModel { get; private set; }
    public Vector2 lastSpawnPosition;

    private void Awake()
    {
        PlayerModel = GetComponentInChildren<PlayerModel>();
    }
}
