using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : Singleton<RespawnObject>
{
    public float respawnDelay = 5f; // Time delay before respawn

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<RespawnObject>(this);
    }

    public IEnumerator RespawnPickup(GameObject p_gameObject)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(respawnDelay);

        p_gameObject.SetActive(true);
    }
}
