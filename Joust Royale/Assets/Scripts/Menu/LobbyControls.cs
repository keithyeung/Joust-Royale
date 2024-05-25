using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyControls : Singleton<LobbyControls>
{
    public List<PlayerInput> playerInputs = new List<PlayerInput>();
    PlayerInputManager playerInputManager;

    [Header("Arena Selection Panel")]
    [SerializeField] private GameObject arenaSelection;
    [Header("GameMode Selection Panel")]
    [SerializeField] private GameObject gameModeSelection;

    [Header("Scene name has to be exactly the SAME!")]
    public string SceneName;

    [SerializeField] private Image titleImage;
    [SerializeField] private Image crowdBackground;
    [SerializeField] private GameObject pressStart;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<LobbyControls>(this);
        playerInputManager = GetComponent<PlayerInputManager>();

        arenaSelection.SetActive(false);
        gameModeSelection.SetActive(false);
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if(titleImage.enabled)
        {
            titleImage.enabled = false;
            crowdBackground.enabled = true;
            pressStart.SetActive(false);
            //crowdBackground.color = new Color(crowdBackground.color.r, crowdBackground.color.g, crowdBackground.color.b, 0.5f);
        }
        pi.transform.SetParent(transform);
        playerInputs.Add(pi);
        var pp = new PlayerProperty();
        pp.playerInput = pi;
        pp.device = pi.devices[0];
        pp.deviceName = pi.devices[0].name;
        ServiceLocator.instance.GetService<PPStorage>().playerProperties.Add(pp);       
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += HandlePlayerJoin;
    }
    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= HandlePlayerJoin;
    }


    public void ReadyPlayer(int index)
    {
        var pp_inStorage = ServiceLocator.instance.GetService<PPStorage>().playerProperties;
        pp_inStorage[index].isReady = true;
        
        if(AllPlayerReady())
        {
            gameModeSelection.SetActive(true);
        }
    }

    public void GameModeToArenaSelection(GameMode.GameModes p_gameMode)
    {
        ServiceLocator.instance.GetService<PPStorage>().SetGameMode(p_gameMode);
        gameModeSelection.SetActive(false);
        arenaSelection.SetActive(true);
    }

    public bool AllPlayerReady()
    {
        return ServiceLocator.instance.GetService<PPStorage>().playerProperties.All(p => p.isReady == true);
    }

    public void SelectionOfArena(string name)
    {
        ServiceLocator.instance.GetService<PPStorage>().SetArenaName(name);
        SceneManager.LoadScene(SceneName);
    }

    

    public void DisableBackground()
    {
        crowdBackground.enabled = false;
    }
}
