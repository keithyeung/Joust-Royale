using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button plumeStealerButton;
    [SerializeField] private Button deathMatchButton;
    [SerializeField] private Button crownSnatcherButton;

    [Header("Rules")]
    [SerializeField] private TextMeshProUGUI ps_rules;
    [SerializeField] private TextMeshProUGUI dm_rules;
    [SerializeField] private TextMeshProUGUI cs_rules;

    private ISelectable selectable;

    public enum GameModes
    {
        PlumeStealer,
        DeathMatch,
        CrownSnatcher
    }
    private GameModes gameMode;

    private void Start()
    {
        selectable = GetComponent<ISelectable>();
    }

    public void SelectionOfGameMode()
    {
        ServiceLocator.instance.GetService<LobbyControls>().GameModeToArenaSelection(gameMode);
    }

    private void StateMachine(GameModes mode)
    {
        
        switch (gameMode)
        {
            case GameModes.PlumeStealer:
                ps_rules.gameObject.SetActive(true);
                dm_rules.gameObject.SetActive(false);
                cs_rules.gameObject.SetActive(false);
                break;
            case GameModes.DeathMatch:
                ps_rules.gameObject.SetActive(false);
                dm_rules.gameObject.SetActive(true);
                cs_rules.gameObject.SetActive(false);
                break;
            case GameModes.CrownSnatcher:
                ps_rules.gameObject.SetActive(false);
                dm_rules.gameObject.SetActive(false);
                cs_rules.gameObject.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectable.currentSelectedObject == plumeStealerButton.gameObject)
        {
            gameMode = GameModes.PlumeStealer;
        }
        else if (selectable.currentSelectedObject == deathMatchButton.gameObject)
        {
            gameMode = GameModes.DeathMatch;
        }
        else if (selectable.currentSelectedObject == crownSnatcherButton.gameObject)
        {
            gameMode = GameModes.CrownSnatcher;
        }

        StateMachine(gameMode);
    }
}
