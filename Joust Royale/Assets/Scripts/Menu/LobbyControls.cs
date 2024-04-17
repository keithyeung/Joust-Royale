using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyControls : Singleton<LobbyControls>
{
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    PlayerInputManager playerInputManager;

    private void Awake()
    {
        SingletonBuilder(this);
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("player joined " + pi.playerIndex);
        pi.transform.SetParent(transform);
        playerInputs.Add(pi);
        var pp_inStorage = ServiceLocator.instance.GetService<PPStorage>().playerProperties;
        var pp = new PlayerProperty();
        pp.playerInput = pi;
        pp_inStorage.Add(pp);
        Debug.Log("Storage size: " + pp_inStorage.Count);
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += HandlePlayerJoin;
    }
    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= HandlePlayerJoin;
    }
   
}
