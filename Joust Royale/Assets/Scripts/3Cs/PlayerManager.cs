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
    private GameState gameStateManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        gameStateManager = FindAnyObjectByType<GameState>();
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
        //using parent due to prefab structure
        Transform playerParent = player.transform.parent;
        //Vector3 oldParentPosition = playerParent.transform.position;
        players.Add(player);
        playerParent.transform.position = playerSpawnPositions[players.Count - 1].position;        


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

        
        //Set the color of Plumage UI based on the material
        PlayerHealth playerHealthComponent = playerParent.GetComponentInChildren<PlayerHealth>();
        int materialIndex = players.Count - 1;
        if (materialIndex < playerMaterials.Count)
        {
            //GameObject temptorso = playerParent.GetComponentInChildren<PlayerController>().torso;
            //GameObject tempFrontHorseCape = playerParent.GetComponentInChildren<PlayerController>().frontHorseCape;
            //GameObject tempBackHorseCape = playerParent.GetComponentInChildren<PlayerController>().backHorseCape;
            //temptorso.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            //tempFrontHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            //tempBackHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            SetPlayerColor(playerParent, materialIndex);
            if (playerMaterials[materialIndex].HasProperty("_color"))
            {
                Color tempColor = playerMaterials[materialIndex].GetColor("_color");
                playerHealthComponent.SetPlumageColor(tempColor);
            }
            else
            {
                Debug.Log("Does not have the _color property");
            }
        }

        SetPlayerPlumagePrefab(player);

        gameStateManager.UpdateWinCount();
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

    private void SetPlayerColor(Transform playerParentTransform, int materialIndex)
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
