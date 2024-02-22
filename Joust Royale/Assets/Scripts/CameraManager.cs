using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Camera> playerCameras; // List to store player cameras

    void Start()
    {
        // Initialize the cameras and adjust split-screen based on the number of active players
        SetSplitScreen(GetActivePlayerCameras());
    }

    void Update()
    {
        // Check for changes in active players and update split-screen accordingly
        List<Camera> activePlayerCameras = GetActivePlayerCameras();

        if (activePlayerCameras.Count != playerCameras.Count)
        {
            SetSplitScreen(activePlayerCameras);
        }
    }

    // Function to set split-screen based on the active players' cameras
    void SetSplitScreen(List<Camera> activePlayerCameras)
    {
        int activePlayers = activePlayerCameras.Count;

        if (activePlayers >= 2)
        {
            if (activePlayers == 4)
            {
                SetFourPlayerSplitScreen();
            }
            else // Distribute the screens equally among active players
            {
                for (int i = 0; i < activePlayers; i++)
                {
                    float screenPercentage = 1f / activePlayers;
                    activePlayerCameras[i].rect = new Rect(i * screenPercentage, 0, screenPercentage, 1);
                }
            }
        }
        else if (activePlayers == 1)
        {
            // If only one player is active, use the full screen
            activePlayerCameras[0].rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            // Handle the case when there are no active players or an unsupported number
            Debug.LogWarning("Unsupported number of players: " + activePlayers);
        }
    }

    // Helper function to get the list of active player cameras
    List<Camera> GetActivePlayerCameras()
    {
        // Filter the active player cameras from the list
        return playerCameras.FindAll(camera => camera.gameObject.activeSelf);
    }

    void SetFourPlayerSplitScreen()
    {
        // Set split-screen for four players
        playerCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        playerCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        playerCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
        playerCameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
    }
}
