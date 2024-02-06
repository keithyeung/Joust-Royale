using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class LegsController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 20f;
    public float brakeLerpSpeed = 5f;

    private float targetBreakingForce = 0f;

    private float currentTurnAngle = 0f;
    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;


    private void FixedUpdate()
    {
        // W/S acceleration from vertical axis
        currentAcceleration = acceleration * (Keyboard.current.wKey.isPressed ? 1f : (Keyboard.current.sKey.isPressed ? -1f : 0f));

        // Set target braking force based on Space key or Gamepad input
        targetBreakingForce = Keyboard.current.spaceKey.isPressed || Gamepad.current != null ? breakingForce : 0f;

        // Smoothly interpolate current braking force towards the target
        currentBreakingForce = Mathf.Lerp(currentBreakingForce, targetBreakingForce, Time.fixedDeltaTime * brakeLerpSpeed);

        //If space is pressed, apply breaking force.
        //if (Keyboard.current.spaceKey.isPressed)
        //{
        //    currentBreakingForce = breakingForce;
        //}
        //else
        //{
        //    currentBreakingForce = 0f;
        //}

        //Apply acceleration to front wheels.
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakingForce;
        frontLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;

        //Take care of steering
        currentTurnAngle = maxTurnAngle * (Keyboard.current.dKey.isPressed ? 1f : (Keyboard.current.aKey.isPressed ? -1f : 0f));
        frontRight.steerAngle = currentTurnAngle;
        frontLeft.steerAngle = currentTurnAngle;
    }

}
