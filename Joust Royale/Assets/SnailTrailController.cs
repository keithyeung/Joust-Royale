using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailTrailController : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other = null)
        {
            return;
        }
        string message = other.name + " entered slime";
        Debug.Log(message);
    }
}
