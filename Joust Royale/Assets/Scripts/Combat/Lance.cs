using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Lance : MonoBehaviour
{
    private LayerMask thisLayer;
    private PlumageManager plumageManager;
    private Shield shield;
    private TestController testController;

    [Header("Scripts that need help")]
    [SerializeField] private PlayerState playerState;


    [Header("VFX")]
    [SerializeField] private GameObject tip;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem splinters;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private ParticleSystem longSmoke;
    [SerializeField] private ParticleSystem longSplinters;

    [Header("Hit vibration")]
    [SerializeField] private float hit_lowFrequency = 1.0f;
    [SerializeField] private float hit_highFrequency = 1.0f;
    [SerializeField] private float hit_duration = 0.2f;

    [Header("Parried vibration")]
    [SerializeField] private float parried_lowFrequency = 1.0f;
    [SerializeField] private float parried_highFrequency = 1.0f;
    [SerializeField] private float parried_duration = 0.5f;

    private void Start()
    {
        thisLayer = GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        plumageManager = GetComponentInParent<PlumageManager>();
        playerState = GetComponentInParent<PlayerState>();
        testController = playerState.GetComponentInChildren<TestController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //maybe check the layer here, so I dont need the 2 if statement down below.
        if (playerState.state != PLAYER_STATE.Attacking) return;

        if (other.gameObject.CompareTag("Armor"))
        {
            HandleArmorCollision(other);
            //UpdateDataStatus(other);

        }
        if (other.gameObject.CompareTag("Shield"))
        {
            HandleShieldCollision(other);
        }
    }

    private void UpdateDataStatus(Collider other)
    {
        var enemyPlayerController = other.gameObject.GetComponentInParent<PlayerController>();
        var enemyTestController = enemyPlayerController.GetComponentInChildren<TestController>();

        testController.accumulatedHits++;
        enemyTestController.accumulatedHitsReceived++;
        testController.IsPositionInsideCollider(testController.transform.position);
        enemyTestController.IsPositionInsideCollider(testController.transform.position);
    }

    private void PlayParticleAtTip(ParticleSystem particleSystem)
    {
        particleSystem.transform.position = tip.transform.position;
        particleSystem.Play();
    }

    private void PlayParticleAlongEdge(ParticleSystem particleSystem)
    {
        particleSystem.transform.position = transform.position;
        particleSystem.transform.rotation = transform.rotation;

        particleSystem.Play();
    }

    private void HandleArmorCollision(Collider other)
    {
        PlayerController otherPlayerController = other.GetComponentInParent<PlayerController>();
        LayerMask tempLayer = other.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();
        PlumageManager opponentPlumageManager = other.gameObject.GetComponentInParent<PlumageManager>();

        PlayParticleAtTip(sparks);

        if (tempLayer == thisLayer) return;

        GameObject tempMaterial = other.gameObject.GetComponentInParent<PlayerHealth>().plumagePrefabInPlayer;
        if (tempMaterial == null || opponentPlumageManager == null)
        {
            Debug.Log("Lance.cs cannot find a material");
            return;
        }
        //adding vibration to the enemy controller
        otherPlayerController.VibrateControllerIfPossible(hit_lowFrequency, hit_highFrequency, hit_duration);

        //blanking out the material
        other.gameObject.GetComponentInParent<PlayerHealth>().StartInvincibility();
        
        //handling the game mode
        HandleGameMode(otherPlayerController, opponentPlumageManager);
    }


    private void HandleShieldCollision(Collider other)
    {
        Shield shield = other.gameObject.GetComponent<Shield>();
        LayerMask tempLayer = other.gameObject.GetComponentInParent<PlayerController>().GetLayerMaskForArmor();

        var enemyPlayerController = other.gameObject.GetComponentInParent<PlayerController>();
        var enemyTestController = enemyPlayerController.GetComponentInChildren<TestController>();

        if (shield != null && tempLayer != thisLayer && shield.isParryActive)
        {
            //longSmoke.Play();
            //longSplinters.Play();
            PlayParticleAlongEdge(longSmoke);
            PlayParticleAlongEdge(longSplinters);
            enemyTestController.accumulatedHitsParried++;

            //controller shake stuff
            var playerController = GetComponentInParent<PlayerController>();
            playerController.VibrateControllerIfPossible(parried_lowFrequency, parried_highFrequency, parried_duration);

            ServiceLocator.instance.GetService<AudioManager>().Play("SuccessfulParry");
            Debug.Log("Lance is broken");
            this.gameObject.SetActive(false);

        }
    }

    
    public void PlayTrail(bool play) { if (play) trail.Play(); else trail.Stop(); }

    private void HandleGameMode(PlayerController playerController, PlumageManager opponentPlumageManager)
    {
        GameMode.GameModes gameMode = ServiceLocator.instance.GetService<GameRules>().gameModes;
        switch (gameMode)
        {
            case GameMode.GameModes.PlumeStealer:
                HandlePlumeStealerMode(playerController, opponentPlumageManager);
                break;
            case GameMode.GameModes.DeathMatch:
                HandleDeathMatchMode(playerController, opponentPlumageManager);
                break;
            case GameMode.GameModes.CrownSnatcher:
                HandleCrownSnatcherMode(playerController, opponentPlumageManager);
                break;
            default:
                Debug.Log("GameMode not found");
                break;
        }
    }

    private void HandlePlumeStealerMode(PlayerController playerController, PlumageManager opponentPlumageManager)
    {
        if (opponentPlumageManager.GetPlumageCount() > 0)
        {
            Color plumeColor = opponentPlumageManager.StealPlume();
            plumageManager.AddPlume(plumeColor);
            ServiceLocator.instance.GetService<AudioManager>().Play("GotHit");
        }
        ServiceLocator.instance.GetService<GameState>().CheckForCrown();
    }

    private void HandleDeathMatchMode(PlayerController playerController, PlumageManager opponentPlumageManager)
    {
        Color plumeColor = opponentPlumageManager.StealPlume();
        if (opponentPlumageManager.GetPlumageCount() > 0)
        {
            ServiceLocator.instance.GetService<AudioManager>().Play("GotHit");
        }
        else
        {
            playerController.CheckDMmatchRules();
            ServiceLocator.instance.GetService<AudioManager>().Play("DeathSFX");
        }
    }

    private void HandleCrownSnatcherMode(PlayerController playerController, PlumageManager opponentPlumageManager)
    {
        Debug.Log("CrownSnatcherMode");
        // steal crown if there's one otherwise stun them/ hit them away
        if(playerController.crown.activeSelf)
        {
            playerController.crown.SetActive(false);
            GetComponentInParent<PlayerController>().crown.SetActive(true);
            ServiceLocator.instance.GetService<AudioManager>().Play("Snatch");

        }
        ServiceLocator.instance.GetService<AudioManager>().Play("GotHit");
        playerController.StunPlayerForDuration(2.0f);
        
    }

    
}


//back up:
//prob statemachine??
//if (ServiceLocator.instance.GetService<GameRules>().gameModes == GameMode.GameModes.PlumeStealer)
//{
//    //Everything about the plume
//    if (other.GetComponentInParent<PlumageManager>().GetPlumageCount() > 0)
//    {
//        Color plumeColor = opponentPlumageManager.StealPlume();
//        plumageManager.AddPlume(plumeColor);
//        ServiceLocator.instance.GetService<AudioManager>().Play("GotHit");
//    }
//    ServiceLocator.instance.GetService<GameState>().CheckForCrown();
//}
//else if (ServiceLocator.instance.GetService<GameRules>().gameModes == GameMode.GameModes.DeathMatch)
//{
//    if (other.GetComponentInParent<PlumageManager>().GetPlumageCount() > 0)
//    {
//        Color plumeColor = opponentPlumageManager.StealPlume();
//        ServiceLocator.instance.GetService<AudioManager>().Play("GotHit");
//        if (other.GetComponentInParent<PlumageManager>().GetPlumageCount() == 0)
//        {
//            //play dead animation
//            //Maybe ragdoll it.
//        }
//    }
//}
//else if (ServiceLocator.instance.GetService<GameRules>().gameModes == GameMode.GameModes.CrownSnatcher)
//{
//    // steal crown if there's one otherwise stun them/ hit them away

//}
//else
//{
//    Debug.Log("GameMode not found");
//}