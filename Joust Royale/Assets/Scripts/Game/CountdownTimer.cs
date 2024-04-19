using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] float remainingTime = 180f;


    private void FixedUpdate()
    {
        if(ServiceLocator.instance.GetService<GameState>() != null)
        {
            if (ServiceLocator.instance.GetService<GameState>().states == GameState.GameStatesMachine.Ended) return;
        }
        if(remainingTime <= 0)
        {
            remainingTime = 0;      
            ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Ended;
            ServiceLocator.instance.GetService<CSVWriter>().WriteToCSV();
            return;
        }
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
