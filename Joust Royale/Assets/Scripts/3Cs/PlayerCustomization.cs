using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCustomization : MonoBehaviour
{
    [System.Serializable]
    public class HelmetSelection
    {
        public string name;
        public GameObject gameObject;
    }

    public HelmetSelection[] helmets;

    [SerializeField] private HelmetSelection currentHelmet;

    private void Start()
    {
        HandleDefaultHelmet();
    }

    private void HandleDefaultHelmet()
    {
        if (currentHelmet != null)
        {
            return;
        }
        for (int i = 0; i < helmets.Length; i++)
        {
            if (helmets[i].gameObject.GetComponent<MeshRenderer>().enabled)
            {
                currentHelmet.gameObject = helmets[i].gameObject;
                return;
            }
        }
        if (currentHelmet == null)
        {
            helmets[0].gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    

    public void SetCurrentHelmet(string helmetName)
    {
        foreach (var helmet in helmets)
        {
            if (helmet.name == helmetName)
            {
                currentHelmet.gameObject.GetComponent<MeshRenderer>().enabled = false;
                currentHelmet.gameObject = helmet.gameObject;
                currentHelmet.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public GameObject GetCurrentHelmet()
    {
        return currentHelmet.gameObject;
    }

    public string GetCurrentHelmetName()
    {
        return currentHelmet.name;
    }
}
