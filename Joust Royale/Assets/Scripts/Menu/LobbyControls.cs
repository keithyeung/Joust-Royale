using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LobbyControls : Singleton<LobbyControls>
{
    public List<PlayerInput> playerInputs = new List<PlayerInput>();
    PlayerInputManager playerInputManager;

    [Header("Arena Selection Panel")]
    [SerializeField] private GameObject arenaSelection;

    [Header("Scene name has to be exactly the SAME!")]
    public string SceneName;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<LobbyControls>(this);
        playerInputManager = GetComponent<PlayerInputManager>();

        arenaSelection.SetActive(false);
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
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
        //if (pp_inStorage.All(p => p.isReady == true))
        //{
        //    arenaSelection.SetActive(true);
        //    //SceneManager.LoadScene(SceneName);
        //}
        if(AllPlayerReady())
        {
            arenaSelection.SetActive(true);
        }
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
}
