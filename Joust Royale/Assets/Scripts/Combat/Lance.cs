using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private LayerMask thisLayer;

    private void Start()
    {
        thisLayer = GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Armor"))
        {
            LayerMask tempLayer = collision.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
            Debug.Log(thisLayer + " , " + tempLayer);
            if (tempLayer == thisLayer)
            {
                Debug.Log("Lance hit friendly");
            }
            else
            {
                Debug.Log("Lance hit enemy");
            }
            
        }
    }
}
