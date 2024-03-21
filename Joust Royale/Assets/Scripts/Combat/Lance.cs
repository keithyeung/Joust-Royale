using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private LayerMask thisLayer;
    private PlayerKillCount playerKillCount;

    private void Start()
    {
        thisLayer = GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        playerKillCount = GetComponentInParent<PlayerKillCount>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Armor"))
        {
            LayerMask tempLayer = collision.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
            if (tempLayer != thisLayer)
            {
                Material tempMaterial = collision.gameObject.GetComponentInParent<PlayerHealth>().plumageMaterialPrefab;
                if(tempMaterial != null)
                {
                    collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage();
                    playerKillCount.AddPlumages(tempMaterial);
                }
            }
        }
    }
}
