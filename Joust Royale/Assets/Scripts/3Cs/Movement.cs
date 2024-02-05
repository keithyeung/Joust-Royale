using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private Vector2 moveInput;

    private int playerIndex; // Index to track which player this script controls

    //Input system and references to camera
    private CustomInput input = null;
    private Vector3 moveVector = Vector3.zero;
    [SerializeField] private Transform playerCamera;

    private void Awake()
    {
        input = new CustomInput();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement is canceled
    }

    private void OnEnable()
    {
        input.Enable();
        //InputSystem.EnableDevice(UnityEngine.InputSystem.Keyboard.current);
        //InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        input.Disable();
        //InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added)
        {
            AssignDeviceToPlayer(device);
        }
        else if (change == InputDeviceChange.Removed)
        {
            // Handle device removal if needed
        }
    }

    private void AssignDeviceToPlayer(InputDevice device)
    {
        // Check if the device is a keyboard or a game controller
        if (device is UnityEngine.InputSystem.Keyboard)
        {
            // Assign keyboard to player 1
            playerIndex = 1;
        }
        else if (device is UnityEngine.InputSystem.Gamepad)
        {
            // Assign game controller to players 2, 3, 4, etc.
            Gamepad gamepad = (Gamepad)device;
            playerIndex = gamepad.deviceId + 2; // Assuming player 1 and 2 are reserved for keyboard and first controller
        }

        Debug.Log($"Assigned device {device.displayName} to Player {playerIndex}");
    }

    private void Update()
    {
        // Move the player based on input
        Vector3 move = ((playerCamera.forward * moveInput.y) + (playerCamera.right * moveInput.x)).normalized;
        move.y = 0;
        transform.Translate(move * moveSpeed * Time.deltaTime);

        RotateTowardsMovementDirection(move);
    }

    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    // Get the movement input value
    //    moveInput = context.ReadValue<Vector3>();
    //}

    private void RotateTowardsMovementDirection(Vector3 moveDirection)
    {
        // Calculate the target rotation based on the movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        // Smoothly interpolate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public CustomInput GetInput()
    {
        return input;
    }
}
