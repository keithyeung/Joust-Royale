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

        if(other.gameObject.CompareTag("Crown"))
        {
            EquipTheCrown(other, GetComponent<PlayerController>().crown, "Found a Crown and equipped");
            ServiceLocator.instance.GetService<AudioManager>().Play("Snatch");
        }
        
    }

    

    private void EquipItem(Collider other, GameObject playerItem, string logMessage)
    {
        //how can I find a specific object in the child of the other gameobject with a name?
        var findShine = other.gameObject.transform.Find("Smart_shine");
        var findLance = findShine.gameObject.transform.Find("Lance");
        Renderer objectRenderer = findLance.gameObject.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            Debug.Log(objectRenderer.name);
            Material tempMaterial = objectRenderer.material;
            playerItem.GetComponent<Renderer>().material = tempMaterial;
        }
        playerItem.SetActive(true);
        ServiceLocator.instance.GetService<AudioManager>().Play("PickUp");
        Debug.Log(logMessage);
        other.gameObject.SetActive(false);
        StartCoroutine(ServiceLocator.instance.GetService<RespawnObject>().RespawnPickup(other.gameObject));
    }

    private void EquipTheCrown(Collider other, GameObject playerItem, string logMessage)
    {
        playerItem.SetActive(true);
        ServiceLocator.instance.GetService<AudioManager>().Play("PickUp");
        Debug.Log(logMessage);
        other.gameObject.SetActive(false);

    }
}
