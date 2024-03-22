using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parrying : MonoBehaviour
{
    public bool isParryActive = false;
    public bool detectedLance = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Lance") && isParryActive == true);
        {
            Debug.Log("Parry Successful");
            Debug.Log(other.gameObject.name);
            other.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("SuccessfulParry");
            FindObjectOfType<AudioManager>().Play("LanceBreak");
        }
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("LT Pressed");
            isParryActive = true;
        }
        else if(context.canceled)
        {
            Debug.Log("LT Released");
            isParryActive = false;
        }
    }

    public void OnReleaseParry(InputAction.CallbackContext context)
    {
        isParryActive = false;
    }

}
