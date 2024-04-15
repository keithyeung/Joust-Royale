using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyControls : Singleton<LobbyControls>
{
    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    private List<PlayerProperty> playerProperty = new List<PlayerProperty>();
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
        var pp = new PlayerProperty();
        pp.playerInput = pi;
        playerProperty.Add(pp);
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += HandlePlayerJoin;
    }
    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= HandlePlayerJoin;
    }
    public void Update()
    {
        //if(playerInputs.Count > 0)
        //{
        //    Debug.Log("Players joined");
        //}
    }
}
