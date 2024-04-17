using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState>
{
    PlayerManager playerManager;
    private int winCount;
    public bool Playtesting = false;
    public enum GameStatesMachine { MainMenu, Playing, Ended};
    public GameStatesMachine states;

    private void Awake()
    {
        playerManager = ServiceLocator.instance.GetService<PlayerManager>();
        states = GameStatesMachine.Playing;
        ServiceLocator.instance.GetService<AudioManager>().Play("BGM");
        SingletonBuilder(this);
    }

    private void Start()
    {
        UpdateWinCount();
    }

    private void Update()
    {
        if (states != GameStatesMachine.Playing) return;

        if (playerManager.players.Count <= 1)
        {
            return;
        }
        foreach (var player in playerManager.players)
        {
            
            if (player.GetComponent<PlumageManager>().GetPlumageCount() >= winCount)
            {
                states = GameStatesMachine.Ended;
                ServiceLocator.instance.GetService<CSVWriter>().WriteToCSV();
                //ServiceLocator.instance.GetService<AudioManager>().Play("Victory");
            }
        }
    }

    public void UpdateWinCount()
    {
        winCount = playerManager.players.Count * 2 + 1;
    }
}
