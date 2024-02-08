using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;

    private void Start()
    {
        // Assign the correct follow target for the Cinemachine Virtual Camera
        playerCamera.Follow = transform;
    }
}
