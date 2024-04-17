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
    [SerializeField]
    private Image panelColor;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    enum PlayerColor
    {
        Red,
        Blue,
        Yellow,
        Green
    }

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
        titleText.SetText("Player " + (playerIndex + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    public void SetPlayerPanelColor(int index)
    {
        playerIndex = index;
        ColorHandler();
    }

    private void ColorHandler()
    {
        switch (playerIndex)
        {
            case (int)PlayerColor.Red:
                panelColor.color = Color.red;
                break;
            case (int)PlayerColor.Blue:
                panelColor.color = Color.blue;
                break;
            case (int)PlayerColor.Yellow:
                panelColor.color = Color.yellow;
                break;
            case (int)PlayerColor.Green:
                panelColor.color = Color.green;
                break;
        }
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
        
        ServiceLocator.instance.GetService<LobbyControls>().ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
