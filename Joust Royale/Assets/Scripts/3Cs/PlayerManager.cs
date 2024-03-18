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
    [SerializeField] private List<Material> playerMaterials;
    [SerializeField] private List<Transform> playerSpawnPositions;

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
        Vector3 oldParentPosition = playerParent.transform.position;
        playerParent.transform.position = playerSpawnPositions[players.Count - 1].position;
        // Calculate the difference in positions
        Vector3 positionDifference = playerParent.transform.position - oldParentPosition;
        Debug.Log("Pos diff: " + positionDifference);
        // Apply the difference in positions to the child object
        //player.GetComponent<Transform>().position -= positionDifference;
        player.transform.position = new Vector3(1000, 100001, 1000);
        Debug.Log("playerPosition: " + player.GetComponent<Transform>().position);


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

        

        PlayerHealth playerHealthComponent = playerParent.GetComponentInChildren<PlayerHealth>();
        int materialIndex = players.Count - 1;
        if (materialIndex < playerMaterials.Count)
        {
            GameObject temptorso = playerParent.GetComponentInChildren<PlayerController>().torso;
            GameObject tempHorseCape = playerParent.GetComponentInChildren<PlayerController>().horseCape;
            temptorso.GetComponent<Renderer>().material = playerMaterials[materialIndex];
            tempHorseCape.GetComponent<Renderer>().material = playerMaterials[materialIndex];
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

        player.transform.position = new Vector3(0, 0.61f, 0);

    }

}
