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
    private GameState gameState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        gameState = ServiceLocator.instance.GetService<GameState>();
    }

    private void FixedUpdate()
    {
        if (!gameState)
        {
            return;
        }
        switch (gameState.states)
        {
            case GameState.GameStatesMachine.Ended:
                return;
            case GameState.GameStatesMachine.MainMenu:
                countdownText.color = Color.red;
                PreGameTimer();
                break;
            case GameState.GameStatesMachine.Playing:
                countdownText.color = Color.black;
                GameplayTimer();
                break;
        }
    }

    private void GameplayTimer()
    {
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            countdownText.text = "00:00";
            GameState.instance.states = GameState.GameStatesMachine.Ended;
            CSVWriter.instance.WriteToCsv();
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
        switch (preGameTime)
        {
            //countdownText.color = Color.red;
            case <= 0f:
                preGameTime = 0;
                PlayerManager.instance.DisablePlayerJoining();
                gameState.states = GameState.GameStatesMachine.Playing;
                return;
            case <= 1f when animator.enabled != true:
                animator.enabled = true;
                break;
        }

        preGameTime -= Time.deltaTime;
        var minutes = Mathf.FloorToInt(preGameTime / 60f);
        var seconds = Mathf.FloorToInt(preGameTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
