using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Shield : MonoBehaviour
{
    public bool isParryActive = false;

    public enum ShieldStatus { Idle, Parry, Block, TiredBlock, Cooldown };
    public ShieldStatus shieldStatus;
    [SerializeField] private PlayerState playerstate;

    [Header("Time for states")]
    [SerializeField] private float parryTime = 0.5f;
    [SerializeField] private float blockTime = 0.5f;
    [SerializeField] private float tiredBlockingTime = 0.5f;
    [SerializeField] private float cooldownTime = 0.5f;

    private void Start()
    {
        shieldStatus = ShieldStatus.Idle;
    }

    private void HandleShieldFSM()
    {
        switch (shieldStatus)
        {
            case ShieldStatus.Idle:
                break;
            case ShieldStatus.Parry:
                isParryActive = true;
                StartCoroutine(StateTimer(parryTime, ShieldStatus.Cooldown));
                break;
            case ShieldStatus.Block:
                playerstate.animator.SetBool("Blocking", true);
                StartCoroutine(StateTimer(blockTime, ShieldStatus.TiredBlock));
                break;
            case ShieldStatus.TiredBlock:
                StartCoroutine(StateTimer(tiredBlockingTime, ShieldStatus.Cooldown));
                break;
            case ShieldStatus.Cooldown:
                playerstate.animator.SetBool("Blocking", false);
                StartCoroutine(StateTimer(cooldownTime, ShieldStatus.Idle));
                break;
        }
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        if (shieldStatus != ShieldStatus.Idle) return;
        if (context.performed)
        {
            playerstate.animator.Play("ShieldParry");
            shieldStatus = ShieldStatus.Parry;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (shieldStatus != ShieldStatus.Idle) return;
        shieldStatus = ShieldStatus.Block;
        if (context.performed)
        {
            Debug.Log("Block Pressed");
        }
        else if (context.canceled)
        {
            Debug.Log("Block Released");
        }
    }

    public bool IsParrying()
    {
        return isParryActive;
    }

    //A function for timer which takes a float as a parameter
    private IEnumerator StateTimer(float time, ShieldStatus state)
    {
        yield return new WaitForSeconds(time);
        isParryActive = false;
        shieldStatus = state;
    }

    private void FixedUpdate()
    {
        HandleShieldFSM();
    }
}
