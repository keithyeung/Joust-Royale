using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] float remainingTime = 180f;
    [SerializeField] float preGameTime = 5f;



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
            PreGameTimer();
        }
        else if(gameState.states == GameState.GameStatesMachine.Playing)
        {
            GameplayTimer();
        }
    }

    private void GameplayTimer()
    {
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Ended;
            ServiceLocator.instance.GetService<CSVWriter>().WriteToCSV();
            return;
        }
        if (remainingTime <= 10)
        {
            countdownText.color = Color.red;
        }
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void PreGameTimer()
    {
        if (preGameTime <= 0)
        {
            preGameTime = 0;
            ServiceLocator.instance.GetService<PlayerManager>().DisablePlayerJoining();
            ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Playing;
            return;
        }
        preGameTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(preGameTime / 60f);
        int seconds = Mathf.FloorToInt(preGameTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
