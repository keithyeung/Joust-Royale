using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlumageManager : MonoBehaviour
{
    public GameObject plumeObject;
    public Transform plumeTransform;
    public int startAmount;
    public int maxAmount;
    public float spreadAngle;

    
    private Color color = Color.white;
    private List<GameObject> plumes = new List<GameObject>();

    void Start()
    {
        Debug.Log("new plume list created");

        GameObject torso = GetComponent<PlayerController>().torso;
        Material objectMaterial = torso.GetComponent<Renderer>().material;
        color = objectMaterial.GetColor("_base_color");
        
        if(color == null)
        {
            Debug.Log("Color is empty");
            return;
        }
        for (int i = 0; i < startAmount; i++)
        {
            AddPlume(color);
        }
    }

    public int GetPlumageCount()
    {
        return plumes.Count;
    }

    public void AddPlume(Color _color)
    {
        GameObject newPlume = Instantiate(plumeObject, plumeTransform);
        newPlume.GetComponentInChildren<MeshRenderer>().material.SetColor("_color", _color);
        plumes.Add(newPlume);
        ArrangePlumage();
        Debug.Log("player got a new plume: " + GetPlumageCount());
    }

    public Color StealPlume()
    {
        Color stolenPlumeColor = plumes.Last<GameObject>().GetComponentInChildren<MeshRenderer>().material.GetColor("_color");
        RemoveLastPlume();
        ArrangePlumage();
        return stolenPlumeColor;
    }

    void RemoveLastPlume()
    {
        if (plumes.Count > 0)
        {
            plumes.Last<GameObject>().SetActive(false);
            plumes.Last<GameObject>().transform.parent = null;
            plumes.RemoveAt(plumes.Count - 1);
            Debug.Log("player lost a plume: " + GetPlumageCount());
        }
        else
        {
            Debug.Log("There was no plumage");
        }
    }


    public void ArrangePlumage()
    {
        int plumeCount = plumes.Count;

        if (plumeCount == 0)
        {
            return;
        }

        //Quaternion playerRotation = GetComponent<Transform>().rotation;
        Transform transform = GetComponent<Transform>();
        float plumeSpacing = spreadAngle / (plumeCount + 1);
        float startAngle = -(spreadAngle * 0.5f) + plumeSpacing;
        Vector3 playerRotation = transform.rotation.eulerAngles;


        for (int i = 0; i < plumeCount; i++)
        {
            float angle = startAngle + i * plumeSpacing;


            //Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Quaternion rotation = Quaternion.Euler(playerRotation.x, playerRotation.y, playerRotation.z + angle );

            plumes[i].transform.rotation = rotation;
        }
    }
}
