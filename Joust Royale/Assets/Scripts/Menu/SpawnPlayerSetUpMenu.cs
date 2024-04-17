using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;

public class SpawnPlayerSetUpMenu : MonoBehaviour
{
    public GameObject playerSetupMenuPrefab;

    private GameObject rootMenu;
    public PlayerInput input;

    private void Awake()
    {
        rootMenu = GameObject.Find("MainLayout");
        if (rootMenu != null)
        {
            var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
        }

    }
    
}
