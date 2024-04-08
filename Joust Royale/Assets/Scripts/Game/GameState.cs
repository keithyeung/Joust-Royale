using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState>
{
    PlayerManager playerManager;
    private int winCount;
    public enum GameStatesMachine { MainMenu, Playing, Ended};
    public GameStatesMachine states;

    private void Start()
    {
        playerManager = ServiceLocator.instance.GetService<PlayerManager>();
        states = GameStatesMachine.Playing;
        ServiceLocator.instance.GetService<AudioManager>().Play("BGM");
        SingletonBuilder(this);
    }

    private void Update()
    {
        if (playerManager.players.Count <= 1)
        {
            return;
        }
        foreach (var player in playerManager.players)
        {
            
            if (player.GetComponent<PlumageManager>().GetPlumageCount() >= winCount)
            {
                states = GameStatesMachine.Ended;
                //ServiceLocator.instance.GetService<AudioManager>().Play("Victory");
            }
        }
    }

    public void UpdateWinCount()
    {
        winCount = playerManager.players.Count * 2 + 1;
    }
}
