using UnityEngine;

public class SoundTest : MonoBehaviour
{
    // Reference to the AudioSource component
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check for input (you can customize the button as needed)
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Call a function to play the sound effect
            FindObjectOfType<AudioManager>().Play("Dash");
        }

        // Check for input (you can customize the button as needed)
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Call a function to play the sound effect
            FindObjectOfType<AudioManager>().Play("TakingDamage");
        }
    }
}
