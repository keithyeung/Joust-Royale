using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    public bool isParryActive = false;

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Lance")
    //    {
    //        Debug.Log("Shield is broken");
    //        this.gameObject.SetActive(false);
    //        FindObjectOfType<AudioManager>().Play("ShieldBreak");
    //    }
    //}


    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Lance"))
    //    {
    //        Debug.Log("Parry Shield is detecting Lance");
    //        if(isParryActive)
    //        {
    //            Debug.Log("Parried!");
    //            other.gameObject.SetActive(false);
    //            FindObjectOfType<AudioManager>().Play("LanceBreak");
    //            FindObjectOfType<AudioManager>().Play("SuccessfulParry");
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Lance"))
        {
            Debug.Log("Parry Shield is no longer detecting Lance");
        }
    }
    public void OnParry(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("LT Pressed");
            isParryActive = true;
        }
        else if (context.canceled)
        {
            Debug.Log("LT Released");
            isParryActive = false;
        }
    }

    public bool IsParrying()
    {
        return isParryActive;
    }
}
