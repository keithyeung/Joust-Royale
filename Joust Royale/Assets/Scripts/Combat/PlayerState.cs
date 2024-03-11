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
            case PLAYER_STATE.Dead:
                break;
            default:
                break;
        }

        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        state = PLAYER_STATE.Attacking;
    }

    public void OnReleaseAttack(InputAction.CallbackContext context)
    {
        state = PLAYER_STATE.Idle;
        animator.SetBool("AttackMode", false);
    }

    //Write a function to check if a key is pressed and hold and return true if it does. so it should take a KeyCode as a parameter and return a bool
    private bool IsKeyPressed(KeyCode key)
    {
        return Input.GetKey(key);
    }
    
    public PLAYER_STATE SetState(PLAYER_STATE newState)
    {
        return state = newState;
    }
     

    
}
