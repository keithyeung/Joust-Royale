using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerKillCount : MonoBehaviour
{
    public List<GameObject> Plumages;
    public int plumageNumber;
    [SerializeField] private GameObject plumageInPlayer;

    // Start is called before the first frame update
    void Start()
    {
        plumageNumber = 3;
        plumageInPlayer = gameObject.GetComponent<PlayerHealth>().plumagePrefabInPlayer;
        foreach (GameObject plumage in Plumages)
        {
            plumage.GetComponent<MeshRenderer>().sharedMaterial = plumageInPlayer.GetComponent<MeshRenderer>().sharedMaterial;
        }
        for (int i = 0; i < plumageNumber; i++)
        {
            Plumages[i].SetActive(true);
        }
    }

    public int GetPlumageCount()
    {
        return Plumages.Count;
    }

    public void AddPlumages(Material material)
    {
        plumageNumber++;
        Plumages[plumageNumber - 1].GetComponent<MeshRenderer>().sharedMaterial = material;
        Plumages[plumageNumber - 1].SetActive(true);
        Debug.Log("We have set the color");
    }

    public void RemovePlumages()
    {
        if(Plumages.Count <= 0)
        {
            Debug.Log("We have no plumages to remove");
            return;
        }
        Plumages[plumageNumber -1].SetActive(false);
        plumageNumber--;
    }

    
}
