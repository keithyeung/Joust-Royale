using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProperty
{
    public PlayerInput playerInput;
    public InputDevice device;
    public string deviceName;
    public int playerIndex;
    public bool isReady;
    public Vector3 position;
    public Material material;
    public LayerMask layer;
    public Color color;
    public PlayerCustomization.HelmetSelection helmetSelection;
}
