using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("Shield Script Loaded");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lance")
        {
            Debug.Log("Shield is broken");
            this.gameObject.SetActive(false);
        }
        //Debug.Log(collision.gameObject.name);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lance"))
        {
            Debug.Log("Parry Shield is broken");
            this.gameObject.SetActive(false);
        }
    }

}
