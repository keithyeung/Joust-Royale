using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Object_shine : MonoBehaviour
{
    private Material objectMaterial;

    void Start()
    {
        // Get the material of the child object's Renderer
        MeshRenderer childRenderer = GetComponentInChildren<MeshRenderer>();

        if (childRenderer == null)
        {
            Debug.LogError("No Renderer found in child objects.");
            return;
        }

        objectMaterial = childRenderer.material;
        if (objectMaterial == null)
        {
            Debug.LogError("No Material found on the child object's Renderer.");
            return;
        }

        // Get the color from the material
        Color color;
        if (objectMaterial.HasProperty("_base_color"))
        {
            color = objectMaterial.GetColor("_base_color");
        }
        else
        {
            Debug.LogError("Material does not have a _base_color property.");
            return;
        }

        // Set the particle system material color
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            Renderer particleRenderer = particleSystem.GetComponent<Renderer>();
            if (particleRenderer != null)
            {
                Material particleMaterial = new Material(particleRenderer.material);
                particleMaterial.SetColor("_color", color);
                particleRenderer.material = particleMaterial;
            }
        }
        else
        {
            Debug.LogWarning("No ParticleSystem component found.");
        }

        // Set the light color
        Light lightComponent = GetComponent<Light>();
        if (lightComponent != null)
        {
            lightComponent.color = color;
        }
        else
        {
            Debug.LogWarning("No Light component found.");
        }

        Debug.Log("New shine colored by object material.");
    }
}


