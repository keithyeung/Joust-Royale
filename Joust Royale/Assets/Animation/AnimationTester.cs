using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    private void Start()
    {
        // Get reference to the Animator component
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Example: Trigger animation clip when a key is pressed
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Trigger animation clip named "MyAnimationClip"
            animator.Play("HelmetTurn");
        }
    }
}
