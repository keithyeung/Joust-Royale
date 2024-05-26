using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameRules : Singleton<GameRules>
{
    public GameMode.GameModes gameModes;
    [SerializeField]
    private PlayerManager playerManager;

    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<GameRules>(this);

        if(ServiceLocator.instance.GetService<PPStorage>() != null)
        {
            gameModes = ServiceLocator.instance.GetService<PPStorage>().gameMode;
        }
        else 
            gameModes = GameMode.GameModes.PlumeStealer;
    }

    private void Start()
    {

    }

    public void CheckWinCondition()
    {
        switch (gameModes)
        {
            case GameMode.GameModes.PlumeStealer:
                //To get plumes: Charge at your opponents and hit them with your lance.
                //Win condition: Most plumes or all the plumes.
                //Default mode

                //When you hit someone, you steal their plumes.
                break;
            case GameMode.GameModes.DeathMatch:
                //Win Condition: The one with plumes left or the one with most plumes left at the end of the time.
                //Health: 3 plumes (feathers on the helmet resemble health points).
                //Deal damage: Charge at your opponents and hit them with your lance.

                //when you hit someone, you destroy their plumes.
                //foreach (var player in playerManager.players)
                //{
                //    if(playerManager.players.Count == 1)
                //    {
                //        ServiceLocator.instance.GetService<GameState>().states = GameState.GameStatesMachine.Ended;
                //    }
                //}

                break;
            case GameMode.GameModes.CrownSnatcher:
                //Win Condition: The one who has owned the crown the most.
                //For every second a player has the crown they get one point.
                //Snatch the Crown: Charge at your opponents and hit them with your lance.

                //When you hit someone, you steal their crown if they have one, otherwise you push them away/stun them.
                break;
            default:
                break;
        }
    }

}
