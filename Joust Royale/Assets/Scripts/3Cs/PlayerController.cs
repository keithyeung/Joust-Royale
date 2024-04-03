using Cinemachine;
using Unity.VisualScripting;
//using UnityEditor.iOS.Extensions.Common;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Horse tilting Related")]
    [SerializeField] public float maxTiltAngle = 30f; // Maximum angle the motorcycle can tilt
    [SerializeField] public float minTiltSpeed = 5f;  // Minimum tilt speed
    [SerializeField] public float maxTiltSpeed = 15f; // Maximum tilt speed
    [SerializeField] public float speedMultiplier = 0.1f; // Multiplier to control the effect of speed on tilt speed

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
    [SerializeField] private PlayerState playerState;

    //Hard coded things
    private Vector3 previousPosition;
    private bool resetPosition = false;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerState = GetComponent<PlayerState>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (ServiceLocator.instance.GetService<GameState>().states != GameState.GameStatesMachine.Playing) return;

        GroundPlayer();

        HandleMovement();

        ResetPlayerPositionIfNeeded();
    }

    private void GroundPlayer()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    private void ResetPlayerPositionIfNeeded()
    {
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
            ApplyRegularMovement();
        }

        ApplyRotation();
        ApplyMovement();
        ApplyGravity();
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

    private void ApplyRotation()
    {
        //float up_rotation = movementInput.x * rotationSpeed * Time.deltaTime;
        //transform.Rotate(Vector3.up * up_rotation);

        float turnInput = movementInput.x;
        float rotationAngle = turnInput * rotationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);

        Quaternion tilt = GetHorseTilt();

      
        //transform.rotation = transform.rotation * rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation * rotation, tilt, Time.deltaTime * 5f);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);



    }

    private Quaternion GetHorseTilt()
    {
        float turnInput = movementInput.x;
        float targetTiltAngle = Mathf.Lerp(0, maxTiltAngle, currentSpeed) * turnInput;

        if (Mathf.Approximately(turnInput, 0))
        {
            targetTiltAngle = 0f;
        }

        targetTiltAngle = Mathf.Clamp(targetTiltAngle, -maxTiltAngle, maxTiltAngle);
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -targetTiltAngle);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        return targetRotation;
    }





    private void ApplyMovement()
    {
        Vector3 moveDirection = transform.forward * currentSpeed * Time.deltaTime;
        controller.Move(moveDirection);
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }



}
