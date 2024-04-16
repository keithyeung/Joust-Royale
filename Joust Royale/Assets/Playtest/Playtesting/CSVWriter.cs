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
        DateTime currentTime = DateTime.Now;

        string[] rowData = { "Player Name", " Player Plumage " , "Accumulated Attempt to hit" ,
            "Accumulated PlayerHit Number", "Accumulated PlayerHitReceived Number", "Accumulated Player Standing Still Time" ,
            "Accumulated Zone A Engagement", "Accumulated Zone B Engagement", "Accumulated Zone C Engagement",
            "Accumulated Zone D Engagement", "Accumulated MidCircle Engagement",
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
        var playtestVariable = player.GetComponentInChildren<TestController>();
        // Player Data
        int playerEngagement = playtestVariable.accumulatedInteractions;
        int playerHits = playtestVariable.accumulatedHits;
        int playerHitsReceived = playtestVariable.accumulatedHitsReceived;
        // Zones Data
        int ZoneAInteractions = playtestVariable.zoneInteractions["Zone_A"];
        int ZoneBInteractions = playtestVariable.zoneInteractions["Zone_B"];
        int ZoneCInteractions = playtestVariable.zoneInteractions["Zone_C"];
        int ZoneDInteractions = playtestVariable.zoneInteractions["Zone_D"];
        int MidCircleInteractions = playtestVariable.zoneInteractions["MiddleCircleZone"];
        // Passive time
        var playerStandingStillTime = player.GetComponent<PlayerController>().standStillTime;

        string[] rowData = { playerName, plumageCount.ToString(), playerEngagement.ToString(),
            playerHits.ToString(), playerHitsReceived.ToString() , playerStandingStillTime.ToString(),
            ZoneAInteractions.ToString(), ZoneBInteractions.ToString(), ZoneCInteractions.ToString(),
            ZoneDInteractions.ToString(), MidCircleInteractions.ToString()
         };
        string rowDataString = string.Join(",", rowData);

        tw.WriteLine(rowDataString);
    }

    //private void ZoneData(PlayerInput player)
    //{
    //    var playtestVariable = player.GetComponentInChildren<TestController>();
    //}
}
