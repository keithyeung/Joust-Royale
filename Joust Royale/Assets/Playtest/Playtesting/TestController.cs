using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class TestController : MonoBehaviour
{
    

    [HideInInspector]
    public int accumulatedInteractions = 0;
    [HideInInspector]
    public int accumulatedHits = 0;
    [HideInInspector]
    public int accumulatedHitsReceived = 0;
    //[HideInInspector]
    //public int accumulated_ZoneA_Interactions = 0;
    //[HideInInspector]
    //public int accumulated_ZoneB_Interactions = 0;
    //[HideInInspector]
    //public int accumulated_ZoneC_Interactions = 0;
    //[HideInInspector]
    //public int accumulated_ZoneD_Interactions = 0;
    //[HideInInspector]
    //public int accumulated_MidCircle_Interactions = 0;

    private void Start()
    {
        triggerColliderAssignment();
    }

    public Dictionary<string, int> zoneInteractions = new Dictionary<string, int>
    {
        {"Zone_A", 0},
        {"Zone_B", 0},
        {"Zone_C", 0},
        {"Zone_D", 0},
        {"MiddleCircleZone", 0}
    };

    public string WHereIm;
    public List<Collider> triggerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(gameObject.GetComponentInParent<PlayerState>().state != PLAYER_STATE.Attacking) return;

            HandleZoneData(other); // only need to send in other because the script has it's own data.
            accumulatedInteractions++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerState>().state != PLAYER_STATE.Attacking 
                               && gameObject.GetComponentInParent<PlayerState>().state != PLAYER_STATE.Attacking) return;

        }
    }

    private void HandleZoneData(Collider other)
    {
        IsPositionInsideCollider(transform.position);
        other.gameObject.GetComponentInChildren<TestController>().IsPositionInsideCollider(other.transform.position);
        if (zoneInteractions.ContainsKey(WHereIm))
        {
            zoneInteractions[WHereIm]++;
            //other.gameObject.GetComponentInChildren<TestController>().zoneInteractions[other.gameObject.GetComponentInChildren<TestController>().WHereIm]++;
        }
        else
        {
            Debug.LogWarning("Unknown zone: " + WHereIm);
        }

    }

    private void triggerColliderAssignment()
    {
        GameObject Zones = GameObject.Find("Zones");
        // Check if the GameObject was found
        if (Zones != null && Zones.activeInHierarchy)
        {
            Transform parentZone = Zones.transform;
            for (int i = 0; i < parentZone.childCount; i++)
            {
                Collider collider = parentZone.GetChild(i).GetComponent<Collider>();
                triggerCollider.Add(collider);
            }
        }
        else
        {
            Debug.LogError("GameObject" + Zones.name + "Zone not found!");
        }
    }

    public void GetZonesEngagement()
    {
        int zoneA_Engagement = zoneInteractions["Zone_A"];
        int zoneB_Engagement = zoneInteractions["Zone_B"];
        int zoneC_Engagement = zoneInteractions["Zone_C"];
        int zoneD_Engagement = zoneInteractions["Zone_D"];
        int zoneMiddleCircle_Engagement = zoneInteractions["MiddleCircleZone"];

        Debug.Log(GetComponentInParent<PlayerController>().gameObject.name);
        Debug.Log("Zone A Interaction: " + zoneA_Engagement);
        Debug.Log("Zone B Interaction: " + zoneB_Engagement);
        Debug.Log("Zone C Interaction: " + zoneC_Engagement);
        Debug.Log("Zone D Interaction: " + zoneD_Engagement);
        Debug.Log("Zone Middle Circle Interaction: " + zoneMiddleCircle_Engagement);
    }

    private void Update()
    {
        //if I pressed Space
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //GetZonesEngagement();
        }
    }

    public string IsPositionInsideCollider(Vector3 position)
    {
        if (triggerCollider == null)
        {
            return WHereIm = "Trigger collider reference is not set.";
        }

        for(int i = 0; i < triggerCollider.Count; i++)
        {
            if (triggerCollider[i].bounds.Contains(position))
            {
                return WHereIm = triggerCollider[i].name;
            }
        }

        return "Error";
    }
}
