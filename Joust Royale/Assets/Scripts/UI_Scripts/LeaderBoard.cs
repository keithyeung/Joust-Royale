using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;

public class LeaderBoard : Singleton<LeaderBoard>
{
    public GameObject panel;
    public List<Image> playerIcons;
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<LeaderBoard>(this);
        animator.enabled = false;

        HideAllPlayerIcons();
        panel.SetActive(false);
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

        UpdateLeaderBoardData();

        ShowPlayerIcons(playerCount);
        animator.enabled = true;
    }

    public void UpdateLeaderBoardData()
    {
        List<PlayerInput> playerInputs = ServiceLocator.instance.GetService<PlayerManager>().players;

        List<LeaderBoardData> leaderboardData = new List<LeaderBoardData>();

        foreach (var playerInput in playerInputs)
        {
            LeaderBoardData playerData = CreatePlayerData(playerInput);
            leaderboardData.Add(playerData);
        }

        leaderboardData = SortLeaderboardData(leaderboardData);

        UpdateUIWithLeaderboardData(leaderboardData);
    }

    private LeaderBoardData CreatePlayerData(PlayerInput playerInput)
    {
        LeaderBoardData playerData = new LeaderBoardData();

        playerData.playerIconColor = playerIcons[playerInput.playerIndex].color;
        playerData.plumesNumber = playerInput.GetComponent<PlumageManager>().GetPlumageCount();

        TestController testController = playerInput.GetComponentInChildren<TestController>();
        playerData.attemptHits = testController.accumulatedInteractions;
        playerData.hitsMade = testController.accumulatedHits;
        playerData.parried = testController.accumulatedHitsParried;

        return playerData;
    }

    private List<LeaderBoardData> SortLeaderboardData(List<LeaderBoardData> data)
    {
        return data.OrderByDescending(d => d.plumesNumber).ToList();
    }

    private void UpdateUIWithLeaderboardData(List<LeaderBoardData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            Image playerIcon = playerIcons[i];
            TextMeshProUGUI[] textMeshes = playerIcons[i].gameObject.GetComponentsInChildren<TextMeshProUGUI>();

            playerIcon.color = data[i].playerIconColor;
            textMeshes[0].text = data[i].plumesNumber.ToString();
            textMeshes[1].text = data[i].attemptHits.ToString();
            textMeshes[2].text = data[i].hitsMade.ToString();
            textMeshes[3].text = data[i].parried.ToString();
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
}

struct LeaderBoardData
{
    public Color playerIconColor;
    public int plumesNumber;
    public int attemptHits;
    public int hitsMade;
    public int parried;
}