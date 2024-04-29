using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : Singleton<LeaderBoard>
{
    public GameObject panel;
    public List<GameObject> players;
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<LeaderBoard>(this);
        panel.SetActive(false);
        animator.enabled = false;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        
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

        for (int i = 0; i < playerCount; i++)
        {
            players[i].SetActive(true);
        }
        animator.enabled = true;
    }
    



}
