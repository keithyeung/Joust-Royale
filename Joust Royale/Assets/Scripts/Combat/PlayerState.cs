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
            case PLAYER_STATE.Attacking:
                animator.SetBool("AttackMode", true);
                playerController.rotationSpeed = lowerRotationSpeed;
                break;
            default:
                break;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (ServiceLocator.instance.GetService<GameState>().states != GameState.GameStatesMachine.Playing) return;

        state = PLAYER_STATE.Attacking;
        Lance playerLance = playerController.lance.GetComponent<Lance>();
        playerLance.PlayTrail(true);
        playerController.PlayTrail(true);

        //if (playerController.lance.activeInHierarchy)
        //{
        //}
        //else
        //{
        //    Debug.Log("No Lance equipped");
        //}
        //audioManager.Play("LanceAttack");
    }

    public void OnReleaseAttack(InputAction.CallbackContext context)
    {
        state = PLAYER_STATE.Idle;
        animator.SetBool("AttackMode", false);
        Lance playerLance = playerController.lance.GetComponent<Lance>();
        playerLance.PlayTrail(false);
        playerController.PlayTrail(false);
    }
    
    public PLAYER_STATE SetState(PLAYER_STATE newState)
    {
        return state = newState;
    }

    public PLAYER_STATE GetState()
    {
        return state;
    }

}
