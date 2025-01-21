using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameState : Singleton<GameState>
{
    [SerializeField]
    private PlayerManager playerManager;
    private int winCount;
    [FormerlySerializedAs("Playtesting")] public bool playtesting = false;
    private bool hasShownLeaderBoard = false;
    public enum GameStatesMachine { MainMenu, Playing, Ended}
    public GameStatesMachine states;

    
    //Audio
    AudioManager audioManager;

    [SerializeField] private int frameRate = 60;

    //Code that needed to load stuff in functions
    private GameMode.GameModes gameMode;

    private void Awake()
    {
        Application.targetFrameRate = frameRate;
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<GameState>(this);
        audioManager = ServiceLocator.instance.GetService<AudioManager>();
        audioManager.Play("BGM");
        audioManager.Play("Count");
        
        //Load stuff
        gameMode = ServiceLocator.instance.GetService<GameRules>().gameModes;
        
    }

    private void stateMachine()
    {
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
                if (!hasShownLeaderBoard)
                {
                    audioManager.Play("Victory");
                    ServiceLocator.instance.GetService<LeaderBoard>()?.ShowLeaderBoard();
                    hasShownLeaderBoard = true;
                }
                break;
            default:
                break;
        }
    }

    private void HandlePSmode()
    {
        if (playerManager.players.Count <= 1) return;
        foreach (var player in playerManager.players)
        {
            if (!(player.GetComponent<PlumageManager>()?.GetPlumageCount() >= winCount)) continue;
            states = GameStatesMachine.Ended;
            ServiceLocator.instance.GetService<CSVWriter>().WriteToCsv();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void HandleDMmode()
    {
        if (playerManager.activePlayer > 1) return;
        states = GameStatesMachine.Ended;
        Debug.Log("One player left in DM mode.");
    }

    // ReSharper disable Unity.PerformanceAnalysis
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
