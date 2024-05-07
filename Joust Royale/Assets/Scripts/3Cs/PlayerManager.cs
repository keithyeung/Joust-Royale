using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private readonly int maxPlayer = 4;

    public List<PlayerInput> players = new List<PlayerInput>(); 
    [SerializeField] private List<LayerMask> playerLayers;
    [SerializeField] private List<Material> playerMaterials;
    [SerializeField] private List<GameObject> plumagePrefabList;
    
    private List<Transform> playerSpawnPositions = new List<Transform>();
    public string levelName; 

    [SerializeField] private Camera mainCamera;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    private PlayerInputManager playerInputManager;
    private string[] names = { "Player Red", "Player Blue", "Player Yellow", "Player Green"};

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<PlayerManager>(this);
        SpawnPointsPrefixs();

        if(FindAnyObjectByType<PPStorage>() != null) // if the game started from lobby
        {
            var playerStorageVariable = ServiceLocator.instance.GetService<PPStorage>();
            SetUpArena(playerStorageVariable.GetArenaName());
            if(playerStorageVariable.playerProperties.Count > 0)
            {
                LoadPlayer();
                ServiceLocator.instance.GetService<PPStorage>()?.ClearPlayerProperties();
            }

        }
        else // if it's started from the scene. Basically developer mode.
        {
            playerInputManager.EnableJoining();
            playerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            playerInputManager.onPlayerJoined += AddPlayer;
        }
    }

    private void LoadPlayer()
    {
        var playerList = ServiceLocator.instance.GetService<PPStorage>().playerProperties;

        if (playerList == null) return;

        for (int i = 0; i < playerList.Count; i++)
        {
            var playerInput = PlayerInput.Instantiate(playerPrefab, playerIndex: i);
            playerInput.SwitchCurrentControlScheme(playerList[i].device); // Replace the problematic line
            AddPlayer(playerInput);
        }
        playerInputManager.DisableJoining();
    }

   

    public void AddPlayer(PlayerInput player)
    {
        
        player.transform.position = new Vector3(0f, 0f, 0f);
        players.Add(player);
        Debug.Log("Player Joined" + player.playerIndex);
        //using parent due to prefab structure
        Transform playerParent = player.transform.parent;
        playerParent.transform.position = playerSpawnPositions[players.Count - 1].position;

        SetPlayerLayers(player);
        SetPlayerCamera(player);
        SetPlayerInputHandler(player);
        SetPlayerColor(playerParent);
        SetPlayerPlumagePrefab(player);

        if (playerInputManager.joinBehavior == PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed)
        {
            ServiceLocator.instance.GetService<GameState>().UpdateWinCount();
        }

        if(mainCamera.GetComponent<Camera>().enabled)
        {
            mainCamera.GetComponent<Camera>().enabled = false;
        }

    }

    public void SpawnPointsPrefixs()
    {
        GameObject spawnPointsParent = GameObject.Find("SpawnPointsFamily");
        levelName = spawnPointsParent.transform.parent.name;

        // Check if the GameObject was found
        if (spawnPointsParent != null && spawnPointsParent.activeInHierarchy)
        {
            Transform parentTransform = spawnPointsParent.transform;

            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform childTransform = parentTransform.GetChild(i);
                playerSpawnPositions.Add(childTransform);
            }
        }
        else
        {
            Debug.LogError("GameObject" + spawnPointsParent.name + "SpawnPointsFamily not found!");
        }
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
        player.name = names[players.Count -1];
        Transform armor = player.transform.Find("Mount/Knight/Upper/Knight_Upper 1");
        Transform shield = player.transform.Find("Mount/Knight/Upper/ShieldFolder");
        if (armor != null)
        {
            armor.gameObject.layer = layerToAdd;
        }
        if(shield != null)
        {
            shield.gameObject.layer = layerToAdd;
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
            temptorso.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempFrontHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempBackHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];

            //Lance
            GameObject tempLance = playerParentTransform.GetComponentInChildren<PlayerController>().lance;
            GameObject tempShield = playerParentTransform.GetComponentInChildren<PlayerController>().shield;
            tempLance.GetComponent<Renderer>().material.SetColor("_color", playerMaterials[materialIndex].GetColor("_color"));
            tempLance.GetComponent<Renderer>().material.SetColor("_base_color", playerMaterials[materialIndex].GetColor("_base_color"));

            //Shield
            tempShield.GetComponent<Renderer>().material = playerMaterials[materialIndex];
        }
    }

    private void SetUpArena(string name)
    {
        ServiceLocator.instance.GetService<ArenaManager>().ChangeArena(name);
    }

    public void SetMainCameraActive()
    {
        mainCamera.GetComponent<Camera>().enabled = true;
    }
}
