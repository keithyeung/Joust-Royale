using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class LeaderBoard : Singleton<LeaderBoard>
{
    public GameObject panel;

    [SerializeField] private TextMeshProUGUI ST_WinCondition;
    public List<LeaderBoardData> leaderboardData = new List<LeaderBoardData>();


    public List<Image> playerIcons;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject wipeOut;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<LeaderBoard>(this);
        animator.enabled = false;

        HideAllPlayerIcons();
        panel.SetActive(false);
        leaderboardData.Clear();
    }

    private void OnDisable()
    {
        panel.SetActive(false);
        animator.enabled = false;
    }

    public void ShowLeaderBoard()
    {
        //using int to enable color in order of the players color, need to change this after player customization i
        int playerCount = ServiceLocator.instance.GetService<PlayerManager>().players.Count;
        ServiceLocator.instance.GetService<PlayerManager>().SetMainCameraActive();

        UpdateLeaderBoardData();

        ShowPlayerIcons(playerCount);
        animator.enabled = true;
        animator.Play("PS_LeaderBoard");
    }

    public void UpdateLeaderBoardData()
    {
        List<PlayerInput> playerInputs = ServiceLocator.instance.GetService<PlayerManager>().players;


        foreach (var playerInput in playerInputs)
        {
            if (playerInput.playerIndex < 0)
            {
                Debug.Log("playerIndex < 0 " + playerInput.name);
                continue;
            }
            LeaderBoardData playerData = CreatePlayerData(playerInput);
            leaderboardData.Add(playerData);
        }

        if(ServiceLocator.instance.GetService<GameRules>().gameModes != GameMode.GameModes.CrownSnatcher)
        {
            ST_WinCondition.text = "Plumes";
            leaderboardData = SortLeaderboardDataByPlumes(leaderboardData);
            UpdateUIWithLeaderboardData_Plumes(leaderboardData);
        }
        else
        {
            ST_WinCondition.text = "Total crown time";
            leaderboardData = SortLeaderboardDataByCrownHoldingTime(leaderboardData);
            UpdateUIWithLeaderboardData_CrownTime(leaderboardData);
        }
    }

    public LeaderBoardData CreatePlayerData(PlayerInput playerInput)
    {
        
        LeaderBoardData playerData = new LeaderBoardData();

        playerData.playerIconColor = playerIcons[playerInput.playerIndex].color;
        playerData.plumesNumber = playerInput.GetComponent<PlumageManager>().GetPlumageCount();
        playerData.crownHoldingTime = (int)playerInput.GetComponent<PlayerController>().ownedCrownTime;
        playerData.playerName = playerInput.GetComponent<PlayerController>().gameObject.name;
        return playerData;
    }

    private List<LeaderBoardData> SortLeaderboardDataByPlumes(List<LeaderBoardData> data)
    {
        return data.OrderByDescending(d => d.plumesNumber).ToList();
    }

    private List<LeaderBoardData> SortLeaderboardDataByCrownHoldingTime(List<LeaderBoardData> data)
    {
        return data.OrderByDescending(d => d.crownHoldingTime).ToList();
    }

    private void UpdateUIWithLeaderboardData_Plumes(List<LeaderBoardData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            Image playerIcon = playerIcons[i];
            TextMeshProUGUI[] textMeshes = playerIcons[i].gameObject.GetComponentsInChildren<TextMeshProUGUI>();

            playerIcon.color = data[i].playerIconColor;
            textMeshes[0].text = data[i].playerName.ToString();
            textMeshes[1].text = data[i].plumesNumber.ToString();
        }
    }

    private void UpdateUIWithLeaderboardData_CrownTime(List<LeaderBoardData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            Image playerIcon = playerIcons[i];
            TextMeshProUGUI[] textMeshes = playerIcons[i].gameObject.GetComponentsInChildren<TextMeshProUGUI>();

            playerIcon.color = data[i].playerIconColor;
            textMeshes[0].text = data[i].playerName.ToString();
            textMeshes[1].text = data[i].crownHoldingTime.ToString();
        }
    }

    private void HideAllPlayerIcons()
    {
        foreach (var playerIcon in playerIcons)
        {
            playerIcon.gameObject.SetActive(false);
        }
    }

    private void ShowPlayerIcons(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            playerIcons[i].gameObject.SetActive(true);
        }
    }

    public void CallTheBackToLobby()
    {
        wipeOut.SetActive(true);
        StartCoroutine(SceneTransitionToMenu());
    }

    private IEnumerator SceneTransitionToMenu()
    {
        yield return new WaitForSeconds(3.0f);
        ServiceLocator.instance.GetService<GameState>().BackToLobby();
    }
}

public struct LeaderBoardData
{
    public Color playerIconColor;
    public int crownHoldingTime;
    public int plumesNumber;
    public string playerName;

}