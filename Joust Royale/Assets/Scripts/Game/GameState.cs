using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    PlayerManager playerManager;
    private int winCount;
    public enum GameStatesMachine { MainMenu, Playing, Ended};
    public GameStatesMachine states;
    private AudioManager audioManager;

    private void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        states = GameStatesMachine.Playing;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (playerManager.players.Count <= 1)
        {
            return;
        }
        foreach (var player in playerManager.players)
        {
            if(player.GetComponent<PlayerKillCount>().plumageNumber >= winCount)
            {
                states = GameStatesMachine.Ended;
                if(audioManager != null)
                {
                    audioManager.Play("Victory");
                }
            }
        }
    }

    public void UpdateWinCount()
    {
        winCount = playerManager.players.Count * 2 + 1;
    }
}
