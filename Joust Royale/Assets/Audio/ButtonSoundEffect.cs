using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundEffect : MonoBehaviour
{
    public Button button; // Reference to the button
    public AudioSource audioSource; // Reference to the AudioSource

    void Start()
    {
        // Ensure the button and audio source are assigned
        if (button != null && audioSource != null)
        {
            // Add a listener to the button to call PlaySound method when clicked
            button.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
