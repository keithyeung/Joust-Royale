using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    public GameObject[] plumageIcon;
    public GameObject plumagePrefabInPlayer;

    public GameObject deathSmoke;

    [SerializeField]
    private TrailRenderer stunEffect;

    [SerializeField]
    private GameObject snail;


    //Invincible functions
    [Header("Invincible variable")]
    private bool isInvincible = false;
    public float invincibilityDuration = 2f;

    //Blinking functions
    [Header("Blinking variables")]
    [SerializeField]private GameObject playerModel;
    [SerializeField]private GameObject playerArmor;
    public float blinkInterval = 0.5f;
    public float blinkDuration = 2f;
    private bool isBlinking = false;

    private bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;
        stunEffect.enabled = false;
    }

    private void Update()
    {
        if(isDead)
        {
            DisableCharacterVisually();
        }
    }

    private void DisableCharacterVisually()
    {
        playerModel.SetActive(false);
        snail.SetActive(false);
    }

    private void ToggleVisibility()
    {
        playerModel.SetActive(!playerModel.activeSelf);
    }

    public void TriggerStunEffect()
    {
        stunEffect.enabled = true;
        stunEffect.time = 1f;
        // Move the GameObject slightly to make the trail visible
        transform.position += new Vector3(0, 0.01f, 0);
    }

    public void StopStunEffect()
    {
        stunEffect.enabled = false;
        stunEffect.time = -1f;
        stunEffect.Clear();
    }

    private void StartBlinking()
    {
        
        if (!isBlinking)
        {
            isBlinking = true;
            InvokeRepeating(nameof(ToggleVisibility), 0f, blinkInterval);
            Invoke(nameof(EndBlinking), blinkDuration);
        }
    }

    private void EndBlinking()
    {
        CancelInvoke(nameof(ToggleVisibility));
        playerModel.SetActive(true); // Ensure the player character is visible when blinking ends
        isBlinking = false;
    }

    public void SetPlumagePrefabMaterial(Material material)
    {
        plumagePrefabInPlayer.GetComponent<MeshRenderer>().material = material;
    }

    public void StartInvincibility()
    {
        var plumesManager = GetComponent<PlumageManager>();
        var gameMode = ServiceLocator.instance.GetService<GameRules>().gameModes;
        if (plumesManager.GetPlumageCount() <= 0 && gameMode == GameMode.GameModes.DeathMatch)
        {
            return;
        }
        isInvincible = true;
        StartBlinking();
        Invoke(nameof(EndInvincibility), invincibilityDuration);
        playerArmor.GetComponent<CapsuleCollider>().enabled = false;
        // Additional visual/audio effects for invincibility can be added here
    }

    private void EndInvincibility()
    {
        isInvincible = false;
        playerArmor.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void TakeDamage()
    {
        currentHealth -= 1f;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Player died");
    }

    public void SetPlumageColor(Color color)
    {
        color.a = 255f;
        for (int i = 0; i < plumageIcon.Length; i++)
        {
            plumageIcon[i].GetComponent<Image>().color = color;
        }
    }

    public void Dead()
    {
        ServiceLocator.instance.GetService<VFX_Manager>().SetDeathSmokePositionAndPlay(this.transform.position);
        ServiceLocator.instance.GetService<AudioManager>().Play("Death");
        ServiceLocator.instance.GetService<PlayerManager>().activePlayer--;
        var playerinput = GetComponent<PlayerInput>();
        var playerdata = ServiceLocator.instance.GetService<LeaderBoard>().CreatePlayerData(playerinput);
        ServiceLocator.instance.GetService<LeaderBoard>().leaderboardData.Add(playerdata);
        isDead = true;
        //DisableCharacterVisually();
    }

}
