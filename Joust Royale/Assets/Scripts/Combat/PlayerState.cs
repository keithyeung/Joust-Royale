using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum PLAYER_STATE
{
    Idle,
    Walking,
    Running,
    Attacking,
    Blocking,
    Parry,
    Dead
}

public class PlayerState : MonoBehaviour
{

    public PLAYER_STATE state;
    public Animator animator;
    private PlayerInput playerInput;
    private PlayerController playerController;
    private float lowerRotationSpeed;
    private float defaultRotationSpeed;

    //private CustomInput controls;

    private void Start()
    {
        state = PLAYER_STATE.Idle;
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        defaultRotationSpeed = playerController.rotationSpeed;
        lowerRotationSpeed = playerController.rotationSpeed * 0.5f;
    }

    private void Update()
    {
        HandleStateMachine(); 
    }

    private void HandleStateMachine()
    {
        switch (state)
        {
            case PLAYER_STATE.Idle:
                playerController.rotationSpeed = defaultRotationSpeed;
                break;
            case PLAYER_STATE.Walking:
                break;
            case PLAYER_STATE.Running:
                break;
            case PLAYER_STATE.Attacking:
                animator.SetBool("AttackMode", true);
                playerController.rotationSpeed = lowerRotationSpeed;
                break;
            case PLAYER_STATE.Blocking:
                //animator.SetBool("AttackMode", false);
                break;
            case PLAYER_STATE.Parry:
                animator.Play("ShieldParry");
                state = PLAYER_STATE.Idle;
                break;
            case PLAYER_STATE.Dead:
                break;
            default:
                break;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (playerController.lance.activeInHierarchy)
        {
            state = PLAYER_STATE.Attacking;
        }
        else
        {
            Debug.Log("No Lance equipped");
        }
        //audioManager.Play("LanceAttack");
    }

    public void OnReleaseAttack(InputAction.CallbackContext context)
    {
        state = PLAYER_STATE.Idle;
        animator.SetBool("AttackMode", false);
    }
    
    public PLAYER_STATE SetState(PLAYER_STATE newState)
    {
        return state = newState;
    }
     

    
}
