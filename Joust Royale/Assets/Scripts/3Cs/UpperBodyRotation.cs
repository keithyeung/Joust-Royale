using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class UpperBodyRotation : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera; 

    CinemachineOrbitalTransposer orbital;
    public float rotationSum;
    public float smooth = 5.0f;

    private void Start()
    {
        orbital = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    void Update()
    {
        if (virtualCamera != null)
        {            
            rotationSum = orbital.m_XAxis.Value;

            Quaternion target = virtualCamera.transform.rotation;

            transform.rotation = Quaternion.Euler(0, (target.eulerAngles.y + 90f), 0);
        }
    }
}
