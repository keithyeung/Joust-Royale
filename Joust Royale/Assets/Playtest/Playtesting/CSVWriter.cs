using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class CSVWriter : MonoBehaviour
{
    private string fileName = "PlayerData.csv";
    bool headerWritten;

    // Start is called before the first frame update
    void Awake()
    {
        fileName = Application.dataPath + "/Playtest/PlayerData.csv";
        headerWritten = false;
    }

    public void WriteToCSV()
    {
        if (!ServiceLocator.instance.GetService<GameState>().Playtesting) return;
        if (ServiceLocator.instance.GetService<PlayerManager>().players.Count > 0)
        {
            List<PlayerInput> players = ServiceLocator.instance.GetService<PlayerManager>().players;

            using (TextWriter tw = new StreamWriter(fileName, true))
            {
                if(!headerWritten)
                {
                    WriteCSVHeader(tw);
                    Debug.Log("Wrote Header");
                }
                foreach (PlayerInput player in players)
                {
                    WritePlayerData(tw, player);
                    Debug.Log("Wrote Context");
                }
            }
        }
    }

    private bool IsFileEmpty(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var reader = new StreamReader(stream))
        {
            reader.ReadLine();
            return reader.ReadLine() == null;
        }
    }

    private void WriteCSVHeader(TextWriter tw)
    {
        //textWriter = new StreamWriter(fileName);
        DateTime currentTime = DateTime.Now;

        string[] rowData = { "Player Name", "Player Position x","y","z", " Player Plumage " , "Status" , "Accumulated Player Engagement" ,
            "Accumulated PlayerHit Number", "Accumulated PlayerHitReceived Number", "Accumulated Player Standing Still Time" , 
            "Time: " + currentTime
        };
        string rowDataString = string.Join(",", rowData);
        tw.WriteLine(rowDataString);
        headerWritten = true;
    }

    public void WritePlayerData(TextWriter tw, PlayerInput player)
    {
        //textWriter = new StreamWriter(fileName);
        string playerName = player.name;
        Vector3 playerPosition = player.transform.position;
        int plumageCount = player.GetComponent<PlumageManager>().GetPlumageCount();
        var status = player.GetComponentInChildren<TestController>().GetStatus();
        var playtestVariable = player.GetComponentInChildren<TestController>();
        int playerEngagement = playtestVariable.accumulatedInteractions;
        int playerHits = playtestVariable.accumulatedHits;
        int playerHitsReceived = playtestVariable.accumulatedHitsReceived;
        var playerStandingStillTime = player.GetComponent<PlayerController>().standStillTime;

        string[] rowData = { playerName, playerPosition.ToString(), plumageCount.ToString(), status.ToString() 
                , playerEngagement.ToString(), playerHits.ToString(), playerHitsReceived.ToString() , playerStandingStillTime.ToString()
         };
        string rowDataString = string.Join(",", rowData);

        tw.WriteLine(rowDataString);
    }
}
