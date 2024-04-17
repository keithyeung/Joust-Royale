using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField] 
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;


    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
        titleText.SetText("Player " + (playerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetColor(Material color)
    {
        if (!inputEnabled) return;
        //ServiceLocator.instance.GetService<LobbyControls>();
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) return;
        
        //ServiceLocator.instance.GetService<PlayerManager>().ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
