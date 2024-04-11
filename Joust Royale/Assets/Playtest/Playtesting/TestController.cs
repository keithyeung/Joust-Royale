using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class TestController : MonoBehaviour
{
    public enum STATUS
    {
        ENGAGE_IN_COMBAT,
        NOT_IN_COMBAT,
        I_HIT_SOMEONE,
        I_GOT_HIT,
    }
    private STATUS status;

    [HideInInspector]
    public int accumulatedInteractions = 0;
    [HideInInspector]
    public int accumulatedHits = 0;
    [HideInInspector]
    public int accumulatedHitsReceived = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerState>().state != PLAYER_STATE.Attacking 
                                              && gameObject.GetComponentInParent<PlayerState>().state != PLAYER_STATE.Attacking) return;

            accumulatedInteractions++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerState>().state != PLAYER_STATE.Attacking 
                               && gameObject.GetComponentInParent<PlayerState>().state != PLAYER_STATE.Attacking) return;

            status = STATUS.ENGAGE_IN_COMBAT;
        }
    }

    public void SetStatus(STATUS _status)
    {
        status = _status;
    }

    public STATUS GetStatus()
    {
        return status;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            status = STATUS.NOT_IN_COMBAT;
        }
    }
}
