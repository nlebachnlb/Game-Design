using Framework.AMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayModel : Model<GamePlayApplication>
{
    public PlayerModel PlayerModel { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        PlayerModel = GetComponentInChildren<PlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
