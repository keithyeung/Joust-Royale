using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public List<PlayerInput> players = new List<PlayerInput>();
    //[SerializeField] private List<Transform> startingPoints;
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] private List<Material> playerMaterials;
    [SerializeField] private List<GameObject> plumagePrefabList;
    public List<Transform> playerSpawnPositions;

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
            mainCamera.GetComponent<Camera>().enabled = false;
        }
    }

    public void AddPlayer(PlayerInput player)
    {
        player.transform.position = new Vector3(0f, 0f, 0f);
        players.Add(player);
        //using parent due to prefab structure
        Transform playerParent = player.transform.parent;
        //Vector3 oldParentPosition = playerParent.transform.position;
        playerParent.transform.position = playerSpawnPositions[players.Count - 1].position;        

        SetPlayerLayers(player);
        SetPlayerCamera(player);
        SetPlayerInputHandler(player);
        SetPlayerColor(playerParent);
        SetPlayerPlumagePrefab(player);
        ServiceLocator.instance.GetService<GameState>().UpdateWinCount();
    }

    private void SetPlayerInputHandler(PlayerInput player)
    {
        Transform playerParent = player.transform.parent;
        InputHandler inputHandler = playerParent.GetComponentInChildren<InputHandler>();
        if (inputHandler != null)
        {
            inputHandler.horizontal = player.actions.FindAction("Look");
        }
    }

    private void SetPlayerLayers(PlayerInput player)
    {
        int layerToAdd = Mathf.FloorToInt(Mathf.Log(playerLayers[players.Count -1 ].value, 2));
        Transform playerParent = player.transform.parent;
        playerParent.gameObject.layer = layerToAdd;
        player.gameObject.layer = layerToAdd;
        Transform armor = player.transform.Find("Mount/Knight/Upper/Knight_Upper 1");
        if (armor != null)
        {
            armor.gameObject.layer = layerToAdd;
        }
    }

    private void SetPlayerCamera(PlayerInput player)
    {
        Transform playerParent = player.transform.parent;
        CinemachineVirtualCamera virtualCamera = playerParent.GetComponentInChildren<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.gameObject.layer = playerParent.gameObject.layer;
        }
        Camera camera = playerParent.GetComponentInChildren<Camera>();
        if (camera != null)
        {
            camera.cullingMask |= 1 << playerParent.gameObject.layer;
        }
    }


    private void SetPlayerPlumagePrefab(PlayerInput player)
    {
        Transform playerParent = player.transform.parent;
        PlayerHealth playerHealthComponent = playerParent.GetComponentInChildren<PlayerHealth>();
        int prefabPlumageIndex = players.Count - 1;
        if (prefabPlumageIndex < plumagePrefabList.Count)
        {
            playerHealthComponent.plumagePrefabInPlayer = plumagePrefabList[prefabPlumageIndex];
        }
    }

    private void SetPlayerColor(Transform playerParentTransform)
    {
        int materialIndex = players.Count - 1;
        if (materialIndex < playerMaterials.Count)
        {
            GameObject temptorso = playerParentTransform.GetComponentInChildren<PlayerController>().torso;
            GameObject tempFrontHorseCape = playerParentTransform.GetComponentInChildren<PlayerController>().frontHorseCape;
            GameObject tempBackHorseCape = playerParentTransform.GetComponentInChildren<PlayerController>().backHorseCape;
            GameObject tempLance = playerParentTransform.GetComponentInChildren<PlayerController>().lance;
            GameObject tempShield = playerParentTransform.GetComponentInChildren<PlayerController>().shield;
            temptorso.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempFrontHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempBackHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempLance.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempShield.GetComponent<Renderer>().material = playerMaterials[materialIndex];
        }

    }
}
