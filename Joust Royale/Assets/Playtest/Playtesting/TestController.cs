using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class TestController : MonoBehaviour
{
    public enum STATUS
    {
        SOMEONE_IS_NEAR_ME_WITH_LANCE_DOWN,
        BORED,
        I_HIT_SOMEONE,
        I_GOT_HIT,
    }
    private STATUS status;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerState>().state != PLAYER_STATE.Attacking 
                               && gameObject.GetComponentInParent<PlayerState>().state != PLAYER_STATE.Attacking) return;

            status = STATUS.SOMEONE_IS_NEAR_ME_WITH_LANCE_DOWN;
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
            status = STATUS.BORED;
        }
    }
}
