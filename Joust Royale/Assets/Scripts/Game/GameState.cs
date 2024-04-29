using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameState : Singleton<GameState>
{
    [SerializeField]
    private PlayerManager playerManager;
    private int winCount;
    public bool Playtesting = false;
    public enum GameStatesMachine { MainMenu, Playing, Ended};
    public GameStatesMachine states;


    private void Awake()
    {
        SingletonBuilder(this);
        //playerManager = ServiceLocator.instance.GetService<PlayerManager>();
        states = GameStatesMachine.Playing;
        ServiceLocator.instance.RegisterService<GameState>(this);
        ServiceLocator.instance.GetService<AudioManager>().Play("BGM");
    }

    private void stateMachine()
    {
        switch (states)
        {
            case GameStatesMachine.MainMenu:
                break;
            case GameStatesMachine.Playing:
                if (playerManager.players.Count <= 1)
                {
                    return;
                }
                foreach (var player in playerManager.players)
                {
                    if (player.GetComponent<PlumageManager>()?.GetPlumageCount() >= winCount)
                    {
                        states = GameStatesMachine.Ended;
                        ServiceLocator.instance.GetService<CSVWriter>().WriteToCSV();
                        //ServiceLocator.instance.GetService<AudioManager>().Play("Victory");
                    }
                }
                break;
            case GameStatesMachine.Ended:
                ServiceLocator.instance.GetService<LeaderBoard>()?.ShowLeaderBoard();
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    Destroy(instance.gameObject);
                    instance = null;
                    SceneManager.LoadScene("Lobby");
                    Debug.Log("Back to Lobby");
                }
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        UpdateWinCount();
    }

    private void Update()
    {
        if (states == GameStatesMachine.Ended)
        {
            //if pressed space

            
        }

        stateMachine();
        //if (states != GameStatesMachine.Playing) return;

        //if (playerManager.players.Count <= 1)
        //{
        //    return;
        //}
        //foreach (var player in playerManager.players)
        //{
        //    if (player.GetComponent<PlumageManager>()?.GetPlumageCount() >= winCount)
        //    {
        //        states = GameStatesMachine.Ended;
        //        ServiceLocator.instance.GetService<CSVWriter>().WriteToCSV();
        //        //ServiceLocator.instance.GetService<AudioManager>().Play("Victory");
        //    }
        //}
    }

    public void UpdateWinCount()
    {
        winCount = playerManager.players.Count * 2 + 1;
    }


    public void CheckForCrown()
    {
        PlayerInput playerWithMostPlumages = null;
        int maxPlumages = 0;

        foreach (var player in playerManager.players)
        {
            int playerPlumages = player.GetComponent<PlumageManager>().GetPlumageCount();
            if (playerPlumages > maxPlumages)
            {
                maxPlumages = playerPlumages;
                playerWithMostPlumages = player;
            }
        }

        // Deactivate all crowns
        foreach (var player in playerManager.players)
        {
            GameObject crown = player.GetComponent<PlayerController>().crown;
            if (crown != null)
            {
                crown.SetActive(false);
            }
        }

        // Activate the crown for the player with the most plumages
        if (playerWithMostPlumages != null)
        {
            GameObject crown = playerWithMostPlumages.GetComponent<PlayerController>().crown;
            if (crown != null)
            {
                crown.SetActive(true);
            }
        }
    }
}
