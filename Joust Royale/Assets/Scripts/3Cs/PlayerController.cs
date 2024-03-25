using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Movement Related")]
    public float rotationSpeed = 60f;
    [SerializeField] private float acceleration = 6f;  // Adjust acceleration as needed
    [SerializeField] private float deceleration = 2f;  // Adjust deceleration as needed
    [SerializeField] private float maxSpeed = 30f;  // Adjust max speed as needed
    [SerializeField] private float AttackingAcceleration = 12f;  // Adjust rotation speed as needed
    [SerializeField] private float AttackingMaxSpeed = 50f;  // Adjust max speed as needed

    [Header("Equipments")]
    [SerializeField] public GameObject lance;
    [SerializeField] public GameObject shield;
    [SerializeField] public GameObject torso;
    [SerializeField] public GameObject frontHorseCape;
    [SerializeField] public GameObject backHorseCape;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    //Game State
    [SerializeField] private GameState gameState;
    [SerializeField] private PlayerState playerState;

    //Hard coded things
    private Vector3 previousPosition;
    private bool resetPosition = false;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        gameState = FindObjectOfType<GameState>();
        playerState = GetComponent<PlayerState>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (gameState.states == GameState.GameStatesMachine.Playing)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }


            HandleMovement();

            // Check if the player's position has changed
            if (transform.localPosition != previousPosition)
            {
                if (!resetPosition)
                {
                    transform.localPosition = previousPosition;
                    resetPosition = true;
                }
            }
        }
    }
    public LayerMask GetLayerMaskForArmor()
    {
        Transform armorTransform = transform.Find("Mount/Knight/Upper/Knight_Upper 1");
        if (armorTransform != null)
        {
            return 1 << armorTransform.gameObject.layer;
        }
        return 0;
    }

    public void HandleMovement()
    {

        if(playerState.state == PLAYER_STATE.Attacking)
        {
            ApplyAttackMovement();
        }
        else
        {
            // Update target speed based on input
            targetSpeed = movementInput.y * maxSpeed;

            // Smoothly adjust the current speed
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }

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

    private void ApplyAttackMovement()
    {
        currentSpeed += AttackingAcceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, AttackingMaxSpeed);
    }

    private void ApplyRegularMovement()
    {
        targetSpeed = movementInput.y * maxSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        DecelerateIfNoInput();
    }

    private void DecelerateIfNoInput()
    {
        if (Mathf.Approximately(movementInput.y, 0f))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
    }

    


}
