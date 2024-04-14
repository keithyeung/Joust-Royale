using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPStorage : Singleton<PPStorage>
{
    public List<PlayerProperty> playerProperties = new List<PlayerProperty>();
    private void Awake()
    {
        SingletonBuilder(this);
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddPlayerProperty(PlayerProperty playerProperty)
    {
        playerProperties.Add(playerProperty);
    }

    public void SetPlayerPropeties_Position(int index, Vector3 position)
    {
        playerProperties[index].position = position;
    }

    public Vector2 GetPlayerPropeties_Position(int index)
    {
        return playerProperties[index].position;
    }

    public void SetPlayerPropeties_Material(int index, Material material)
    {
        playerProperties[index].material = material;
    }
    public Material GetPlayerPropeties_Material(int index)
    {
        return playerProperties[index].material;
    }

    public void SetPlayerPropeties_Layer(int index, LayerMask layer)
    {
        playerProperties[index].layer = layer;
    }

    public LayerMask GetPlayerPropeties_Layer(int index)
    {
        return playerProperties[index].layer;
    }

    public void RemovePlayerProperty(PlayerProperty playerProperty)
    {
        playerProperties.Remove(playerProperty);
    }

    public void ClearPlayerProperties()
    {
        playerProperties.Clear();
    }

    public PlayerProperty GetPlayerProperty(int index)
    {
        return playerProperties[index];
    }

    public int GetPlayerPropertyCount()
    {
        return playerProperties.Count;
    }

    public void SetPlayerProperty(int index, PlayerProperty playerProperty)
    {
        playerProperties[index] = playerProperty;
    }

    public void InsertPlayerProperty(int index, PlayerProperty playerProperty)
    {
        playerProperties.Insert(index, playerProperty);
    }

    public void RemovePlayerPropertyAt(int index)
    {
        playerProperties.RemoveAt(index);
    }


}
