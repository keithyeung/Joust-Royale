using System.Collections;
using System.Linq;
//using UnityEditor.iOS.Extensions.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Movement Related")]
    public float rotationSpeed = 60f;
    [SerializeField] private float acceleration = 6f;  // Adjust acceleration as needed
    [SerializeField] private float deceleration = 2f;  // Adjust deceleration as needed
    [SerializeField] private float maxSpeed = 30f;  // Adjust max speed as needed
    [FormerlySerializedAs("AttackingAcceleration")] [SerializeField] private float attackingAcceleration = 12f;  // Adjust rotation speed as needed
    [FormerlySerializedAs("AttackingMaxSpeed")] [SerializeField] private float attackingMaxSpeed = 50f;  // Adjust max speed as needed
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private ParticleSystem trail;

    [Header("Tilting Related")]
    [SerializeField] private float maxTiltAngle = 30f; // Maximum angle the motorcycle can tilt

    [Header("Equipments")]
    [SerializeField] public GameObject lance;
    [SerializeField] public GameObject shield;
    [SerializeField] public GameObject torso;
    [SerializeField] public GameObject frontHorseCape;
    [SerializeField] public GameObject backHorseCape;
    [SerializeField] public GameObject crown;

    public PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput;


    //[Header("External Collision")]
    //[SerializeField] private GameObject snailCollider;

    //Game State
    [SerializeField] private PlayerState playerState;
    private GameRules gameRules;
    private PlayerHealth playerHealth;
    private PlumageManager plumesManager;
    private bool isStunned = false;

    //Hard coded things
    private Vector3 previousPosition;
    private bool resetPosition = false;

    //data
    public float standStillTime = 0f;
    public float ownedCrownTime = 0;
    private GameState gameState;

    private void Start()
    {
        gameState = ServiceLocator.instance.GetService<GameState>();
        playerInput = GetComponent<PlayerInput>();
        controller = gameObject.GetComponent<CharacterController>();
        playerState = GetComponent<PlayerState>();
        crown.SetActive(false);
        gameRules = ServiceLocator.instance.GetService<GameRules>();
        playerHealth = GetComponent<PlayerHealth>();
        plumesManager = GetComponent<PlumageManager>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!gameState)
        {
            return;
        }
        if (gameState.states != GameState.GameStatesMachine.Playing) return;

        GroundPlayer();

        HandleMovement();

        ResetPlayerPositionIfNeeded();
    }

    private void Update()
    {
        if (gameState.states != GameState.GameStatesMachine.Playing) return;

        if (currentSpeed == 0)
        {
            standStillTime += Time.deltaTime;
        }

        if (!crown.activeSelf) return;
        if(gameRules.gameModes == GameMode.GameModes.CrownSnatcher)
        {
            ownedCrownTime += Time.deltaTime;
        }
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
        if (transform.localPosition == previousPosition) return;
        if (resetPosition) return;
        transform.localPosition = previousPosition;
        resetPosition = true;
    }

    public LayerMask GetLayerMaskForArmor()
    {
        var armorTransform = transform.Find("Mount/Knight/Upper/Knight_Upper 1");
        if (armorTransform != null)
        {
            return 1 << armorTransform.gameObject.layer;
        }
        return 0;
    }

    private void HandleMovement()
    {
        if (isStunned) return;
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

        currentSpeed += attackingAcceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, attackingMaxSpeed);
    }

    private void ApplyRegularMovement()
    {
        var targetSpeed = movementInput.y * maxSpeed;
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
        var turnInput = movementInput.x;
        var rotationAngle = turnInput * rotationSpeed * Time.deltaTime;
        var rotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);
        var tilt = GetTiltQuaternion();

        transform.rotation = Quaternion.Lerp(transform.rotation * rotation, tilt, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private Quaternion GetTiltQuaternion()
    {
        var turnInput = movementInput.x;
        var targetTiltAngle = Mathf.Lerp(0, maxTiltAngle, currentSpeed) * turnInput;

        if (Mathf.Approximately(turnInput, 0))
        {
            targetTiltAngle = 0f;
        }

        targetTiltAngle = Mathf.Clamp(targetTiltAngle, -maxTiltAngle, maxTiltAngle);
        var targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -targetTiltAngle);
        return targetRotation;
    }

    private void ApplyMovement()
    {
        var moveDirection = transform.forward * (currentSpeed * Time.deltaTime);

        // Use a raycast to get the normal of the surface directly below the character
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, controller.height / 2 + 0.1f))
        {
            // Adjust the move direction to follow the slope of the terrain
            moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);
        }

        controller.Move(moveDirection);

        
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void PlayTrail(bool play) { if (play) trail.Play(); else trail.Stop(); }

    public void StunPlayerForDuration(float duration)
    {
        StunPlayer();
        StartCoroutine(UnStunPlayerAfterDelay(duration));
    }

    private void StunPlayer()
    {
        isStunned = true;
        playerState.state = PLAYER_STATE.Idle;
        currentSpeed = 0;
        playerState.SetAnimatorBackToDefault();
        playerHealth.TriggerStunEffect();
    }

    private IEnumerator UnStunPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnStunPlayer();
    }

    private void UnStunPlayer()
    {
        isStunned = false;
        playerHealth.StopStunEffect();
    }

    public void VibrateControllerIfPossible(float lowFrequency, float highFrequency, float duration)
    {
        if (playerInput.devices.FirstOrDefault(d => d is Gamepad) is Gamepad gamepad)
        {
            StartCoroutine(VibrateController(gamepad, lowFrequency, highFrequency, duration));
        }
        else
        {
            Debug.Log("Vibration check is no good");
        }
    }

    private static IEnumerator VibrateController(Gamepad gamepad, float lowFrequency, float highFrequency, float duration)
    {
        gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
        yield return new WaitForSeconds(duration);
        gamepad.SetMotorSpeeds(0, 0);
        //CheckDMMatchRules();
    }

    public void CheckDmMatchRules()
    {
        var gameRule = ServiceLocator.instance.GetService<GameRules>();
        if (gameRule.gameModes != GameMode.GameModes.DeathMatch) return;
        if (plumesManager.GetPlumageCount() > 0) return;
        playerHealth.Dead();
        ServiceLocator.instance.GetService<GameRules>().CheckWinCondition();
        Debug.Log(gameObject.name + " is dead");
    }
}
