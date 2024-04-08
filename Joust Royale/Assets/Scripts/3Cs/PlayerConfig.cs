using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfig : MonoBehaviour
{
    public PlayerConfig(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public int PlayerLayer { get; set; }
    public Material PlayerMaterial { get; set; }
    public bool IsReady { get; set; }
    public GameObject PlumagePrefab { get; set; }
    public Transform SpawnPosition { get; set; }
    public Camera PlayerCamera { get; set; }
}
