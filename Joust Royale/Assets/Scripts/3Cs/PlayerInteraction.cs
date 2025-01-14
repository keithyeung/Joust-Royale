using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    private PlayerController playerController;
    private AudioManager audioManager;
    private RespawnObject respawnObject;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        audioManager = ServiceLocator.instance.GetService<AudioManager>();
        respawnObject = ServiceLocator.instance.GetService<RespawnObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "PU_Lance":
                EquipItem(other, playerController.lance, "Found a lance and equipped", "Smart_shine/Lance");
                break;
            case "Crown":
                EquipTheCrown(other, playerController.crown, "Found a Crown and equipped");
                audioManager.Play("Snatch");
                break;
        }
    }

    //adding the childObjectName to the method so it can be used for more than one object
    private void EquipItem(Collider other, GameObject playerItem, string logMessage, string childObjectName)
    {
        var childObject = other.gameObject.transform.Find(childObjectName);
        Renderer objectRenderer = childObject?.gameObject.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            Debug.Log(objectRenderer.name);
            var tempMaterial = objectRenderer.material;
            playerItem.GetComponent<Renderer>().material = tempMaterial;
        }
        playerItem.SetActive(true);
        audioManager.Play("PickUp");
        Debug.Log(logMessage);
        other.gameObject.SetActive(false);
        StartCoroutine(respawnObject.RespawnPickup(other.gameObject));
    }

    private void EquipTheCrown(Collider other, GameObject playerItem, string logMessage)
    {
        playerItem.SetActive(true);
        ServiceLocator.instance.GetService<AudioManager>().Play("PickUp");
        Debug.Log(logMessage);
        other.gameObject.SetActive(false);

    }
}
