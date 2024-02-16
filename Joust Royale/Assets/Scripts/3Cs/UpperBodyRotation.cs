using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class UpperBodyRotation : MonoBehaviour
{
    [SerializeField]
    private Transform upperBody;  // Assign your upper body transform in the Inspector

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;  // Assign your virtual camera in the Inspector

    CinemachineOrbitalTransposer orbital;
    float rotationSum;

    private void Start()
    {
        orbital = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        rotationSum = orbital.m_XAxis.Value;
    }

    void Update()
    {
        //var orbital = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        if (virtualCamera != null && upperBody != null)
        {
            //upperBody.rotation.y = orbital.m_XAxis.Value;
            rotationSum = 90 + orbital.m_XAxis.Value;
            if(rotationSum > 75)
            {
                rotationSum = 75;
            }
            else if(rotationSum < -75)
            {
                rotationSum = -75;
            }
            upperBody.rotation = Quaternion.Euler(0, rotationSum, 0);

            //// Rotate the upper body to face the same direction as the virtual camera
            //upperBody.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
        }
    }
}
