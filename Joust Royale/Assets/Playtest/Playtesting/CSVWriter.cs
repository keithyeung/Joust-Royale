using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    string path;
    string fileName = "PlayerData.csv";

    // Start is called before the first frame update
    void Awake()
    {
        //path = Application.dataPath;
        fileName = Application.dataPath + "/Playtest/PlayerData.csv";
        Debug.Log("File name: " + fileName + "And Path: " + path);
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

            // Open the file in append mode
            TextWriter tw = new StreamWriter(fileName, true);

            // Check if the file is empty, if so, write the header
            if (new FileInfo(fileName).Length == 0)
            {
                tw.WriteLine("Player Name, Player Position, Player Color, Player Plumage, Player Layer");
            }

            foreach (PlayerInput player in players)
            {
                string playerName = player.name;
                Vector3 playerPosition = player.transform.position;
                string playerLayer = player.gameObject.layer.ToString();

                string[] rowDataTemp = new string[5];
                rowDataTemp[0] = playerName;
                rowDataTemp[1] = playerPosition.ToString();
                rowDataTemp[4] = playerLayer;

                string rowData = string.Join(",", rowDataTemp);
                Debug.Log("Player Data: " + rowData);

                // Write the data to the file
                tw.WriteLine(rowData);
            }

            // Close the file
            tw.Close();
        }
    }

}
