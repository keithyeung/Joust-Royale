using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PU_Lance"))
        {
            Renderer objectRenderer = other.gameObject.GetComponent<Renderer>();
            Material tempMaterial;
            //Get the material from the pick up lance
            if (objectRenderer != null)
            {
                tempMaterial = objectRenderer.material;
                GetComponent<PlayerController>().lance.GetComponent<Renderer>().material = tempMaterial;
            }
            GetComponent<PlayerController>().lance.SetActive(true);
            Debug.Log("Found a lance and equiped");
            other.gameObject.SetActive(false);
            StartCoroutine(FindAnyObjectByType<RespawnObject>().RespawnPickup(other.gameObject)); ;
        }

        if (other.gameObject.CompareTag("PU_Shield"))
        {
            Renderer objectRenderer = other.gameObject.GetComponent<Renderer>();
            Material tempMaterial;
            //Get the material from the pick up lance
            if (objectRenderer != null)
            {
                tempMaterial = objectRenderer.material;
                GetComponent<PlayerController>().shield.GetComponent<Renderer>().material = tempMaterial;
            }
            GetComponent<PlayerController>().shield.SetActive(true);
            Debug.Log("Found a Shield and equiped");
            other.gameObject.SetActive(false);
            StartCoroutine(FindAnyObjectByType<RespawnObject>().RespawnPickup(other.gameObject));
        }
    }

    
}
