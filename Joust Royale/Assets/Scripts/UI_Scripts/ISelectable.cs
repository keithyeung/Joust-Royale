using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class ISelectable : MonoBehaviour
{

    public GameObject currentSelectedObject;

    void Update()
    {
        currentSelectedObject = EventSystem.current.currentSelectedGameObject;
        //Check if the GameObject is being highlighted
        
    }
}
