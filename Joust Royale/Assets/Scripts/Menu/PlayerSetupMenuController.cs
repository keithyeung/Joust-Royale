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

    // Define custom colors as static variables
    //public static Color orange = new Color(1f, 0.5f, 0f, 1f);

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
        float alpha_temp = 230f / 255f;
        Color red_lessA = new Color(1f, 0f, 0f, alpha_temp);
        switch (playerIndex)
        {
            case (int)PlayerColor.Red:
                panelColor.color = red_lessA;
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
        //This below should not be here as I just need it to be called once but I will refactor this later.
        if (ServiceLocator.instance.GetService<LobbyControls>().AllPlayerReady()) 
        {
            //ServiceLocator.instance.GetService<LobbyControls>().DisableBackground();
            this.gameObject.SetActive(false);
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
