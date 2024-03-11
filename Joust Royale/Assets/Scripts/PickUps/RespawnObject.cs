using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public GameObject PU_Lance; 
    public GameObject PU_Shield;
    public float respawnDelay = 5f; // Time delay before respawn

    // Update is called once per frame
    //void Update()
    //{
    //    if(!PU_Lance.activeInHierarchy)
    //    {
            
    //        RespawnPickup(PU_Lance);
    //    }

    //    if (!PU_Shield.activeInHierarchy)
    //    {
    //        RespawnPickup(PU_Shield);
    //    }
    //}

    public IEnumerator RespawnPickup(GameObject p_gameObject)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(respawnDelay);

        p_gameObject.SetActive(true);
        Debug.Log("Actived");
    }
}
