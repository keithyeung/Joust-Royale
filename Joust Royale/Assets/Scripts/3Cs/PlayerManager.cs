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

    [SerializeField] private Camera mainCamera;
    private PlayerInputManager playerInputManager;
    private string[] names = { "Player Red", "Player Blue", "Player Yellow", "Player Green"};

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        SingletonBuilder(this);
        //DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SpawnPointsPrefixs();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
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
        Debug.Log("Player Joined" + player.playerIndex);
        //using parent due to prefab structure
        Transform playerParent = player.transform.parent;
        playerParent.transform.position = playerSpawnPositions[players.Count - 1].position;

        SetPlayerLayers(player);
        SetPlayerCamera(player);
        SetPlayerInputHandler(player);
        SetPlayerColor(playerParent);
        SetPlayerPlumagePrefab(player);
        ServiceLocator.instance.GetService<GameState>().UpdateWinCount();
    }

    public void SpawnPointsPrefixs()
    {
        GameObject spawnPointsParent = GameObject.Find("SpawnPointsFamily");

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
            GameObject tempLance = playerParentTransform.GetComponentInChildren<PlayerController>().lance;
            GameObject tempShield = playerParentTransform.GetComponentInChildren<PlayerController>().shield;
            temptorso.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempFrontHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempBackHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempLance.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempShield.GetComponent<Renderer>().material = playerMaterials[materialIndex];
        }
    }

    //public void ReadyPlayer(int index)
    //{
    //    playerConfigs[index].IsReady = true;
    //    if(playerConfigs.Count == maxPlayer && playerConfigs.All(p => p.IsReady ==true))
    //    {
    //        // Or SceneManager.LoadScene(SceneName);
    //        ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Playing;
    //    }
    //}
}
