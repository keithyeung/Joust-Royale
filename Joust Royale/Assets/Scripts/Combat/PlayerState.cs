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

    private CustomInput controls;

    private void OnEnable()
    {
        controls = new CustomInput();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        state = PLAYER_STATE.Idle;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (state)
        {
            case PLAYER_STATE.Idle:
                break;
            case PLAYER_STATE.Walking:
                break;
            case PLAYER_STATE.Running:
                break;
            case PLAYER_STATE.Attacking:
                animator.SetBool("AttackMode", true);
                break;
            case PLAYER_STATE.Blocking:
                break;
            case PLAYER_STATE.Dead:
                break;
            default:
                break;
        }

        // Check if the "Attack" action is being held
        if (controls.Player.AttackMode.triggered)
        {
            state = PLAYER_STATE.Attacking;
        }
        OnReleaseAttack();
    }

    void OnReleaseAttack()
    {
        if (controls.Player.ReleaseAttack.triggered)
        {
            state = PLAYER_STATE.Idle;
            animator.SetBool("AttackMode", false);
        }
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

    //public void AttackAnimatorSetUp()
    //{
    //    animator.SetBool("AttackMode" , true);
    //}
     

    
}