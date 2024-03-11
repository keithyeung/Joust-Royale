using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    //[SerializeField] private List<Transform> startingPoints;
    [SerializeField] private List<LayerMask> playerLayers;

    [SerializeField] private Camera mainCamera;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
        Debug.Log("Player Joined");
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void Update()
    {
        if(players.Count > 0)
        {
            mainCamera.gameObject.SetActive(false);
        }
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        //using parent due to prefab structure
        Transform playerParent = player.transform.parent;
        //playerParent.position = startingPoints[players.Count - 1].position;

        //convert layer mask (bit) to an integer
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);

        //set the layer of the player
        playerParent.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        player.gameObject.layer = layerToAdd;
        player.transform.Find("Mount").Find("Knight").Find("Upper").Find("Knight_Upper 1").gameObject.layer = layerToAdd; // add layer to the armor.
        //add the layer to the player
        playerParent.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
        //set the action in the custom cinemachine Input Handler
        playerParent.GetComponentInChildren<InputHandler>().horizontal = player.actions.FindAction("Look");
    }

}
