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

    private void Update()
    {
        // Move the player based on input
        Vector3 move = ((playerCamera.forward * moveInput.y) + (playerCamera.right * moveInput.x)).normalized;
        move.y = 0;
        transform.Translate(move * moveSpeed * Time.deltaTime);

    }

    public CustomInput GetInput()
    {
        return input;
    }
}
