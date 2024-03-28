using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private LayerMask thisLayer;
    private PlayerKillCount playerKillCount;
    private Shield shield;
    private PlayerState playerState;
    [SerializeField] private GameObject tip;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem splinters;

    private void Start()
    {
        thisLayer = GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        playerKillCount = GetComponentInParent<PlayerKillCount>();
        playerState = GetComponentInParent<PlayerState>(); 
    }

    private void OnTriggerStay(Collider other) 
    {
        if (playerState.state != PLAYER_STATE.Attacking) return;
        if (other.gameObject.CompareTag("Armor"))
        {
            LayerMask tempLayer = other.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
            PlayerKillCount opponentKillCount = other.gameObject.GetComponentInParent<PlayerKillCount>();

            PlayParticle(sparks);

            if (tempLayer != thisLayer)
            {
                GameObject tempMaterial = other.gameObject.GetComponentInParent<PlayerHealth>().plumagePrefabInPlayer;
                if(tempMaterial != null && opponentKillCount != null)
                {
                    //collision.gameObject.GetComponentInParent<PlayerHealth>().TakeDamage(); Removed due to we dont use health anymore.
                    other.gameObject.GetComponentInParent<PlayerHealth>().StartInvincibility();
                    if(playerKillCount.GetPlumageCount() > 0)
                    {
                        playerKillCount.AddPlumages(tempMaterial.GetComponent<MeshRenderer>().sharedMaterial);
                        opponentKillCount.RemovePlumages();
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
                    ServiceLocator.instance.GetService<AudioManager>().Play("SuccessfulParry");
                    PlayParticle(smoke);
                    PlayParticle(splinters);
                    Debug.Log("Lance is broken");
                }
                //else
                //{
                //    //other.gameObject.SetActive(false);
                //    //FindObjectOfType<AudioManager>().Play("ShieldBreak");
                //    //ServiceLocator.instance.GetService<AudioManager>().Play("ShieldBreak");
                //    //PlayParticle(smoke);
                //    //PlayParticle(splinters);
                //    //Debug.Log("Shield is broken!");
                //}
            }
        }
    }

    private void PlayParticle(ParticleSystem particleSystem)
    {
        particleSystem.transform.position = tip.transform.position;
        particleSystem.Play();
    }
}
