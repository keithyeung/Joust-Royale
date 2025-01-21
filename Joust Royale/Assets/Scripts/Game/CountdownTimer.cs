using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] float remainingTime = 180f;
    [SerializeField] float preGameTime = 5f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void FixedUpdate()
    {
        GameState gameState = ServiceLocator.instance.GetService<GameState>();
        if (gameState == null)
        {
            return;
        }
        if (gameState.states == GameState.GameStatesMachine.Ended) return;

        if(gameState.states == GameState.GameStatesMachine.MainMenu)
        {
            countdownText.color = Color.red;
            PreGameTimer();
        }
        else if(gameState.states == GameState.GameStatesMachine.Playing)
        {
            countdownText.color = Color.black;
            GameplayTimer();
        }
    }

    private void GameplayTimer()
    {
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            countdownText.text = "00:00";
            ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Ended;
            ServiceLocator.instance.GetService<CSVWriter>().WriteToCsv();
            return;
        }
        if (remainingTime <= 10)
        {
            countdownText.color = Color.red;
        }
        remainingTime -= Time.deltaTime;
        var minutes = Mathf.FloorToInt(remainingTime / 60f);
        var seconds = Mathf.FloorToInt(remainingTime % 60f);
        countdownText.text = $"{minutes:00}:{seconds:00}";
    }

    private void PreGameTimer()
    {
        //countdownText.color = Color.red;
        if (preGameTime <= 0f)
        {
            preGameTime = 0;
            ServiceLocator.instance.GetService<PlayerManager>().DisablePlayerJoining();
            ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Playing;
            return;
        }
        if(preGameTime <= 1f && animator.enabled != true)
        {
            animator.enabled = true;
        }
        preGameTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(preGameTime / 60f);
        int seconds = Mathf.FloorToInt(preGameTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
