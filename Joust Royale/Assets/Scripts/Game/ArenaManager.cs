using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : Singleton<ArenaManager>
{
    //The arena will have a name and a gameObject
    [System.Serializable]
    public class Arena
    {
        public string name;
        public GameObject gameObject;
    }
    public GameObject crown;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<ArenaManager>(this);
    }
    //This is a crown that should be activated if it's crown snatcher mode

    private void Start()
    {
        if(currentArena == null)
        {
            for(int i = 0;i< arenas.Length; i++)
            {
                if (arenas[i].gameObject.activeSelf)
                {
                    currentArena = arenas[i].gameObject;
                    return;
                }
            }
        }
    }

    //This is an array of the Arena class that will be used to store all the arenas
    public Arena[] arenas;
    //This is the current arena,self explanatory
    [SerializeField] private GameObject currentArena;

    //This will be used to change the arena
    //I will pass the name of the arena for now until I find a better way to do this
    public void ChangeArena(string name)
    {
        foreach (var arena in arenas)
        {
            //If the name of the arena is the same as the name I passed
            if (arena.name == name)
            {
                //I will set the current arena to the gameObject of the arena
                currentArena = arena.gameObject;
                //I will activate the gameObject
                currentArena.SetActive(true);
            }
            else
            {
                //I will deactivate the gameObject
                arena.gameObject.SetActive(false);
            }
        }
    }
    //This will be used to get the current arena
    public GameObject GetCurrentArena()
    {
        return currentArena;
    }

    public void SetCrownActive()
    {
        if (ServiceLocator.instance.GetService<GameRules>().gameModes == GameMode.GameModes.CrownSnatcher)
        {
            crown.SetActive(true);
        }
        else
        {
            crown.SetActive(false);
        }
    }
}
