using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    public GameObject[] plumageIcon;
    public GameObject plumagePrefabInPlayer;


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


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void ToggleVisibility()
    {
        playerModel.SetActive(!playerModel.activeSelf);
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
        isInvincible = true;
        StartBlinking();
        Invoke(nameof(EndInvincibility), invincibilityDuration);
        playerArmor.GetComponent<BoxCollider>().enabled = false;
        // Additional visual/audio effects for invincibility can be added here
    }

    private void EndInvincibility()
    {
        isInvincible = false;
        playerArmor.GetComponent<BoxCollider>().enabled = true;
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

}
