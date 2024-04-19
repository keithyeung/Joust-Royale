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

    [Header("Scene name has to be exactly the SAME!")]
    public string SceneName;

    private void Awake()
    {
        SingletonBuilder(this);
        playerInputManager = GetComponent<PlayerInputManager>();
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
        //pp_inStorage.Count == playerInputManager.playerCount &&
        if (pp_inStorage.All(p => p.isReady == true))
        {
            SceneManager.LoadScene(SceneName);
            //ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Playing; // put this in the next scene instead.
        }
    }
}
