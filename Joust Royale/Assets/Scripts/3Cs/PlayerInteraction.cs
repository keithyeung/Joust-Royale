using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PU_Lance"))
        {
            EquipItem(other, GetComponent<PlayerController>().lance, "Found a lance and equipped");
        }

        if (other.gameObject.CompareTag("PU_Shield"))
        {

            EquipItem(other, GetComponent<PlayerController>().shield, "Found a Shield and equipped");
        }
    }


    //public void CheckCurrentZone(Collider other)
    //{
    //    if(other.gameObject.CompareTag("Zones"))
    //    {
    //        Debug.Log("Player is in " + other.gameObject.name);
    //    }
    //}

    private void EquipItem(Collider other, GameObject playerItem, string logMessage)
    {
        Renderer objectRenderer = other.gameObject.GetComponentInChildren<Renderer>();
        if (objectRenderer != null)
        {
            Material tempMaterial = objectRenderer.material;
            playerItem.GetComponent<Renderer>().material = tempMaterial;
        }
        playerItem.SetActive(true);
        ServiceLocator.instance.GetService<AudioManager>().Play("PickUp");
        Debug.Log(logMessage);
        other.gameObject.SetActive(false);
        StartCoroutine(ServiceLocator.instance.GetService<RespawnObject>().RespawnPickup(other.gameObject));
    }
}
