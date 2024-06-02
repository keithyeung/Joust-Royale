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
    private bool hasShowenLeaderBoard = false;
    public enum GameStatesMachine { MainMenu, Playing, Ended};
    public GameStatesMachine states;


    [SerializeField] private int frameRate = 60;


    private void Awake()
    {
        Application.targetFrameRate = frameRate;
        SingletonBuilder(this);
        //playerManager = ServiceLocator.instance.GetService<PlayerManager>();
        //states = GameStatesMachine.Playing;
        ServiceLocator.instance.RegisterService<GameState>(this);
        ServiceLocator.instance.GetService<AudioManager>().Play("BGM");
    }

    private void stateMachine()
    {
        var gameMode = ServiceLocator.instance.GetService<GameRules>().gameModes;
        switch (states)
        {
            case GameStatesMachine.MainMenu:
                break;
            case GameStatesMachine.Playing:
                if(gameMode == GameMode.GameModes.PlumeStealer)
                {
                    HandlePSmode();
                }
                else if(gameMode == GameMode.GameModes.DeathMatch)
                {
                    HandleDMmode();
                }
                else if(gameMode == GameMode.GameModes.CrownSnatcher)
                {
                    //nth
                }
                else
                {
                    Debug.Log("This should never happen.");
                }
                
                break;
            case GameStatesMachine.Ended:
                if (!hasShowenLeaderBoard)
                {
                    ServiceLocator.instance.GetService<AudioManager>().Play("Victory");
                    ServiceLocator.instance.GetService<LeaderBoard>()?.ShowLeaderBoard();
                    hasShowenLeaderBoard = true;
                }
                break;
            default:
                break;
        }
    }

    private void HandlePSmode()
    {
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
            }
        }
    }

    private void HandleDMmode()
    {
        if (playerManager.activePlayer <= 1)
        {
            states = GameStatesMachine.Ended;
            Debug.Log("One player left in DM mode.");
        }
    }

    public void BackToLobby()
    {
        Destroy(instance.gameObject);
        instance = null;
        SceneManager.LoadScene("Lobby");
        Debug.Log("Back to Lobby");
        
    }

    private void EmergencyBackToMenu()
    {
        //if leftshift + K is pressed
        if (Keyboard.current.qKey.isPressed)
        {
            BackToLobby();
        }
    }

    private void Update()
    {
        stateMachine();
        EmergencyBackToMenu();
    }

    public void UpdateWinCount()
    {
        winCount = playerManager.players.Count * 3;
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
