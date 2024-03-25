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

    private void EquipItem(Collider other, GameObject playerItem, string logMessage)
    {
        Renderer objectRenderer = other.gameObject.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            Material tempMaterial = objectRenderer.material;
            playerItem.GetComponent<Renderer>().material = tempMaterial;
        }
        playerItem.SetActive(true);
        Debug.Log(logMessage);
        other.gameObject.SetActive(false);
        StartCoroutine(FindAnyObjectByType<RespawnObject>().RespawnPickup(other.gameObject));
    }
}
