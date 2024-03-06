using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]private float jumpHeight = 1.0f;
    [SerializeField]private float gravityValue = -9.81f;

    [Header("Movement Related")]
    [SerializeField]private float rotationSpeed = 60f;
    [SerializeField]private float acceleration = 6f;  // Adjust acceleration as needed
    [SerializeField]private float deceleration = 2f;  // Adjust deceleration as needed
    [SerializeField]private float maxSpeed = 30f;  // Adjust max speed as needed

    //[SerializeField] private CustomInput input = null;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }


    public void HandleMovement()
    {
        // Update target speed based on input
        targetSpeed = movementInput.y * maxSpeed;

        // Smoothly adjust the current speed
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        // Apply car-like movement
        Vector3 moveDirection = new Vector3(0, 0, currentSpeed);
        moveDirection = transform.TransformDirection(moveDirection);

        // Apply rotation for car-like steering
        float rotation = movementInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotation);

        // Move the character with the added velocity
        controller.Move(moveDirection * Time.deltaTime);

        // Decelerate when no forward/backward input is provided
        if (Mathf.Approximately(movementInput.y, 0f))
        {
            // Gradual deceleration
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //lookInput = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        HandleMovement();
    }
}
