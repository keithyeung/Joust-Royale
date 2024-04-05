using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    
    private string fileName = "PlayerData.csv";

    // Start is called before the first frame update
    void Awake()
    {
        fileName = Application.dataPath + "/Playtest/PlayerData.csv";
        //Debug.Log("File name: " + fileName + "And Path: " + path);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            WriteToCSV();
        }
    }

    public void WriteToCSV()
    {
        if (ServiceLocator.instance.GetService<PlayerManager>().players.Count > 0)
        {
            List<PlayerInput> players = ServiceLocator.instance.GetService<PlayerManager>().players;

            if (players.Count == 0)
            {
                Debug.LogWarning("No players found.");
                return;
            }
            using (TextWriter tw = new StreamWriter(fileName, true))
            {
                // If the file is empty, write the header
                if (IsFileEmpty(fileName))
                {
                    WriteCSVHeader(tw);
                    Debug.Log("Wrote Header");
                }

                // Write data for each player
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
        return new FileInfo(filePath).Length == 0;
    }

    private void WriteCSVHeader(TextWriter tw)
    {
        tw.WriteLine(" Player Name" , " Player Position"," Player Plumage "," Player Layer ");
    }

    private void WritePlayerData(TextWriter tw, PlayerInput player)
    {
        string playerName = player.name;
        Vector3 playerPosition = player.transform.position;
        int plumageCount = player.GetComponent<PlumageManager>().GetPlumageCount();
        string playerLayer = LayerMask.LayerToName(player.gameObject.layer);

        string[] rowData = { playerName, playerPosition.ToString(), plumageCount.ToString(), playerLayer };
        string rowDataString = string.Join(",", rowData);

        // Write the player data to the file
        tw.WriteLine(rowDataString);
    }
}
