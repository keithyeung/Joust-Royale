using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;

    public GameObject[] plumageIcon;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        for(int i = 0; i < plumageIcon.Length; i++)
        {
            if (i < currentHealth)
            {
                plumageIcon[i].SetActive(true);
            }
            else
            {
                plumageIcon[i].SetActive(false);
            }
        }
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
}
