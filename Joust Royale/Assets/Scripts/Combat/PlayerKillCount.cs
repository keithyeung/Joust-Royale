using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerKillCount : MonoBehaviour
{
    public List<GameObject> Plumages;
    public int plumageNumber;

    // Start is called before the first frame update
    void Start()
    {
        plumageNumber = 0;
    }

    public void AddPlumages(Material material)
    {
        plumageNumber++;
        //Color tempBaseColor = material.GetColor("_base_color");
        //Color tempColor = material.GetColor("_color");
        //if(tempBaseColor == null || tempColor == null)
        //{
        //    Debug.LogError("These materials have no Color or BaseColor component");
        //}
        //Plumages[plumageNumber - 1].gameObject.GetComponent<Material>().SetColor("_base_color", tempBaseColor);
        //Plumages[plumageNumber - 1].gameObject.GetComponent<Material>().SetColor("_color", tempColor);
        Plumages[Plumages.Count - 1].GetComponent<Renderer>().material = material;
        Plumages[plumageNumber - 1].SetActive(true);
        Debug.Log("We have set the color");
    }

    public void RemovePlumages()
    {
        plumageNumber--;
        Plumages.RemoveAt(Plumages.Count -1);
    }

    
}
