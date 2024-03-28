using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private LayerMask thisLayer;
    private PlumageManager plumeManager;
    private Shield shield;
    [SerializeField] private GameObject tip;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem splinters;

    private void Start()
    {
        thisLayer = GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        plumeManager = GetComponentInParent<PlumageManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Lance collided with", other);
        if (other.gameObject.CompareTag("Armor"))
        {

            LayerMask tempLayer = other.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
            PlumageManager opponentPlumeManager = other.gameObject.GetComponentInParent<PlumageManager>();

            PlayParticle(sparks);

            if (tempLayer != thisLayer)
            {
                GameObject tempMaterial = other.gameObject.GetComponentInParent<PlayerHealth>().plumagePrefabInPlayer;
                if(tempMaterial != null && opponentPlumeManager != null)
                {
                    other.gameObject.GetComponentInParent<PlayerHealth>().StartInvincibility();
                    if(plumeManager.GetPlumageCount() > 0)
                    {
                        Color newPlumeColor = opponentPlumeManager.StealPlume();
                        plumeManager.AddPlume(newPlumeColor);
                        //FindObjectOfType<AudioManager>().Play("GotHit");
                        ServiceLocator.instance.GetService<AudioManager>().Play("GotHit");
                    }
                }
                else
                {
                    Debug.Log("Lance.cs cannot find a material");
                }
            }
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            shield = other.gameObject.GetComponent<Shield>();
            LayerMask tempLayer = other.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
            if (shield != null && tempLayer != thisLayer)
            {
                if(shield.isParryActive)
                {
                    this.gameObject.SetActive(false);
                    //FindObjectOfType<AudioManager>().Play("LanceBreak");
                    ServiceLocator.instance.GetService<AudioManager>().Play("LanceBreak");
                    PlayParticle(smoke);
                    PlayParticle(splinters);
                    Debug.Log("Lance is broken");
                }
                else
                {
                    other.gameObject.SetActive(false);
                    //FindObjectOfType<AudioManager>().Play("ShieldBreak");
                    ServiceLocator.instance.GetService<AudioManager>().Play("ShieldBreak");
                    PlayParticle(smoke);
                    PlayParticle(splinters);
                    Debug.Log("Shield is broken!");
                }
            }
        }
    }

    private void PlayParticle(ParticleSystem particleSystem)
    {
        particleSystem.transform.position = tip.transform.position;
        particleSystem.Play();
    }
}
