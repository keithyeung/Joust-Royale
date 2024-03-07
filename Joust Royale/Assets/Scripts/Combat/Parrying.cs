using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parrying : MonoBehaviour
{
    private bool isParryActive = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Lance") && isParryActive);
        {
            Debug.Log("Parry Successful");
            other.gameObject.SetActive(false);
        }
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        if (context.control.IsPressed())
        {
            Debug.Log("LT Pressed");
            isParryActive = true;
        }
    }

    public void OnReleaseParry(InputAction.CallbackContext context)
    {
        isParryActive = false;
    }

}
