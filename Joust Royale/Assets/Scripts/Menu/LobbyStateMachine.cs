using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


public class LobbyStateMachine : MonoBehaviour
{
    [Header("Scene name has to be exactly the SAME!")]
    public string SceneName;

    [Header("Arena Selection Panel")]
    [SerializeField] private GameObject arenaSelection;
    [Header("GameMode Selection Panel")]
    [SerializeField] private GameObject gameModeSelection;

    public Image titleImage;
    [SerializeField] private Image crowdBackground;
    [SerializeField] private GameObject pressStartToJoin;
    public GameObject pressAToStart;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject creditScene;
    [SerializeField] private GameObject additionalCreditScene;
    //[SerializeField] private GameObject creditSceneEventSystem;

    public enum State
    {
        Menu,
        Start,
        Lobby,
        GameModeSelection,
        ArenaSelection,
    }

    private State currentState;

    private void Awake()
    {
        SwitchState(State.Start);
    }

    private void SwitchState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Start:
                startState();
                // ... (code to handle start state)
                break;
            case State.Menu:

                // ... (code to handle menu state)
                break;
            case State.Lobby:
                LobbyState();
                // ... (code to handle joining state)
                break;
            case State.GameModeSelection:
                GameModeSelectionState();
                // ... (code to handle game mode selection state)
                break;
            case State.ArenaSelection:
                ArenaState();
                // ... (code to handle arena selection state)
                break;
        }
    }
    private void LobbyState()
    {
        LobbyControls lobbyControls = ServiceLocator.instance.GetService<LobbyControls>();
        lobbyControls.EnablePlayerJoining();
        titleImage.enabled = false;
        startScreen.SetActive(false);
        pressAToStart.SetActive(false);
        crowdBackground.enabled = true;
        pressStartToJoin.SetActive(true);
    }

    public void startState()
    {
        Debug.Log("Start button clicked");
        startScreen.SetActive(false);
        arenaSelection.SetActive(false);
        gameModeSelection.SetActive(false);
        blackScreen.SetActive(false);
        menuButton.SetActive(false);
        startScreen.SetActive(false);
        pressStartToJoin.SetActive(false);
        pressAToStart.SetActive(true);
    }

    public void ReadyPlayer(int index)
    {
        var pp_inStorage = ServiceLocator.instance.GetService<PPStorage>().playerProperties;
        pp_inStorage[index].isReady = true;

        if (AllPlayerReady())
        {
            LobbyControls lobbyControls = ServiceLocator.instance.GetService<LobbyControls>();
            lobbyControls.DisablePlayerJoining();
            SwitchState(State.GameModeSelection);
        }
    }

    private void GameModeSelectionState()
    {
        gameModeSelection.SetActive(true);
        menuButton.SetActive(true);
        pressStartToJoin.SetActive(false);
    }

    public void SetGameModeToStorage(GameMode.GameModes p_gameMode)
    {
        ServiceLocator.instance.GetService<PPStorage>().SetGameMode(p_gameMode);
        SwitchState(State.ArenaSelection);
    }

    private void ArenaState()
    {
        gameModeSelection.SetActive(false);
        arenaSelection.SetActive(true);
    }

    public bool AllPlayerReady()
    {
        return ServiceLocator.instance.GetService<PPStorage>().playerProperties.All(p => p.isReady == true);
    }

    public void SelectionOfArena(string name)
    {
        blackScreen.SetActive(true);
        menuButton.SetActive(false);
        blackScreen.GetComponent<Animator>().enabled = true;

        ServiceLocator.instance.GetService<PPStorage>().SetArenaName(name);
        StartCoroutine(SceneTransition());
    }
    public void ReloadScene()
    {
        ServiceLocator.instance.GetService<PPStorage>().playerProperties.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private IEnumerator SceneTransition()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneName);
    }

    public void DisplayCreditScene()
    {
        //creditSceneEventSystem.SetActive(true);
        startScreen.SetActive(false);
        additionalCreditScene.SetActive(false);
        creditScene.SetActive(true);
    }

    public void DisplayAdditionalCredits()
    {
        creditScene.SetActive(false);
        additionalCreditScene.SetActive(true);
    }

    public void BackToStartScene()
    {
        //creditSceneEventSystem.SetActive(false);
        creditScene.SetActive(false);
        additionalCreditScene.SetActive(false);
        startScreen.SetActive(true);
    }

    public void DisableBackground()
    {
        crowdBackground.enabled = false;
    }

    public void SwitchToStart()
    {
        SwitchState(State.Start);
    }

    public void SwitchToLobby()
    {
        SwitchState(State.Lobby);
    }
    public void SwitchToMenu()
    {
        SwitchState(State.Menu);
    }
}
