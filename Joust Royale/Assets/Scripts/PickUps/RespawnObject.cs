using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public float respawnDelay = 5f; // Time delay before respawn


    public IEnumerator RespawnPickup(GameObject p_gameObject)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(respawnDelay);

        p_gameObject.SetActive(true);
        Debug.Log("Actived");
    }
}
