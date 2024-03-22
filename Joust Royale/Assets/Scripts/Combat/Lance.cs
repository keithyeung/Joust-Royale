using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private LayerMask thisLayer;
    private PlayerKillCount playerKillCount;
    [SerializeField] private GameObject tip;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem shieldBreak;

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
            PlayerKillCount opponentKillCount = collision.gameObject.GetComponentInParent<PlayerKillCount>();

            PlayParticle();

            if (tempLayer != thisLayer)
            {
                GameObject tempMaterial = collision.gameObject.GetComponentInParent<PlayerHealth>().plumagePrefabInPlayer;
                if(tempMaterial != null && opponentKillCount != null)
                {
                    //collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(); Removed due to we dont use health anymore.
                    collision.gameObject.GetComponentInParent<PlayerHealth>().StartInvincibility();
                    if(playerKillCount.GetPlumageCount() > 0)
                    {
                        playerKillCount.AddPlumages(tempMaterial.GetComponent<MeshRenderer>().sharedMaterial);
                        opponentKillCount.RemovePlumages();
                        FindObjectOfType<AudioManager>().Play("GotHit");
                    }
                }
                else
                {
                    Debug.Log("Lance.cs cannot find a material");
                }
            }
        }
    }

    private void PlayParticle()
    {
        sparks.transform.position = tip.transform.position;
        sparks.Play();
    }
}
