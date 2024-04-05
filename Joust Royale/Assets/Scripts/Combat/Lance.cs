using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
    private LayerMask thisLayer;
    private PlumageManager plumageManager;
    private Shield shield;
    private PlayerState playerState;
    [SerializeField] private GameObject tip;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem splinters;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private ParticleSystem longSmoke;
    [SerializeField] private ParticleSystem longSplinters;

    private void Start()
    {
        thisLayer = GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        plumageManager = GetComponentInParent<PlumageManager>();
        playerState = GetComponentInParent<PlayerState>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerState.state != PLAYER_STATE.Attacking) return;
        //maybe check the layer here, so I dont need the 2 if statement down below.

        if (other.gameObject.CompareTag("Armor"))
        {
            HandleArmorCollision(other);
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            HandleShieldCollision(other);
        }
    }

    private void PlayParticle(ParticleSystem particleSystem)
    {
        particleSystem.transform.position = tip.transform.position;
        particleSystem.Play();
    }

    private void HandleArmorCollision(Collider other)
    {
        LayerMask tempLayer = other.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        PlumageManager opponentPlumageManager = other.gameObject.GetComponentInParent<PlumageManager>();

        PlayParticle(sparks);

        if (tempLayer == thisLayer) return;

        GameObject tempMaterial = other.gameObject.GetComponentInParent<PlayerHealth>().plumagePrefabInPlayer;
        if (tempMaterial == null || opponentPlumageManager == null)
        {
            Debug.Log("Lance.cs cannot find a material");
            return;
        }

        other.gameObject.GetComponentInParent<PlayerHealth>().StartInvincibility();
        if (other.GetComponentInParent<PlumageManager>().GetPlumageCount() > 0)
        {
            Color plumeColor = opponentPlumageManager.StealPlume();
            plumageManager.AddPlume(plumeColor);
            ServiceLocator.instance.GetService<AudioManager>().Play("GotHit");

        }
    }

    private void HandleShieldCollision(Collider other)
    {
        Shield shield = other.gameObject.GetComponent<Shield>();
        LayerMask tempLayer = other.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        if (shield != null && tempLayer != thisLayer && shield.isParryActive)
        {
            this.gameObject.SetActive(false);
            ServiceLocator.instance.GetService<AudioManager>().Play("SuccessfulParry");
            PlayParticle(longSmoke);
            PlayParticle(longSplinters);
            Debug.Log("Lance is broken");

        }
    }

    public void PlayTrail(bool play)
    {
        if (play)
        {
            trail.Play();

        }
        else
        {
            trail.Stop();
        }
    }
}
