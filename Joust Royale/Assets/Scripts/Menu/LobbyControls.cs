using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyControls : Singleton<LobbyControls>
{
    public List<PlayerInput> playerInputs = new List<PlayerInput>();
    PlayerInputManager playerInputManager;

    //[Header("Arena Selection Panel")]
    //[SerializeField] private GameObject arenaSelection;
    //[Header("GameMode Selection Panel")]
    //[SerializeField] private GameObject gameModeSelection;

    //[Header("Scene name has to be exactly the SAME!")]
    //public string SceneName;

    //public Image titleImage;
    //[SerializeField] private Image crowdBackground;
    //[SerializeField] private GameObject pressStartToJoin;
    //public GameObject pressAToStart;
    //[SerializeField] private GameObject blackScreen;
    //[SerializeField] private GameObject menuButton;
    //[SerializeField] private GameObject startScreen;
    //[SerializeField] private GameObject creditScene;
    //[SerializeField] private GameObject additionalCreditScene;
    ////[SerializeField] private GameObject creditSceneEventSystem;
    

    [SerializeField] private VideoPlaying videoPlaying;


    private void Awake()
    {
        SingletonBuilder(this);
        ServiceLocator.instance.RegisterService<LobbyControls>(this);
        playerInputManager = GetComponent<PlayerInputManager>();

        //arenaSelection.SetActive(false);
        //gameModeSelection.SetActive(false);
        //blackScreen.SetActive(false);
        //menuButton.SetActive(false);
        //startScreen.SetActive(false);
        //pressStartToJoin.SetActive(false);
        //pressAToStart.SetActive(true);
        playerInputManager.DisableJoining();
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        //if (videoPlaying.isPlayingVideo)
        //{
        //    videoPlaying.StopVideo();            
        //}
        //if (titleImage.enabled)
        //{
        //    titleImage.enabled = false;
        //    crowdBackground.enabled = true;
        //    pressStartToJoin.SetActive(false);
        //    //crowdBackground.color = new Color(crowdBackground.color.r, crowdBackground.color.g, crowdBackground.color.b, 0.5f);
        //}
        pi.transform.SetParent(transform);
        playerInputs.Add(pi);
        var pp = new PlayerProperty();
        pp.playerInput = pi;
        pp.device = pi.devices[0];
        pp.deviceName = pi.devices[0].name;
        ServiceLocator.instance.GetService<PPStorage>().playerProperties.Add(pp);       
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += HandlePlayerJoin;
    }
    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= HandlePlayerJoin;
    }

    public bool AllPlayerReady()
    {
        return ServiceLocator.instance.GetService<PPStorage>().playerProperties.All(p => p.isReady == true);
    }

    public void ReloadScene()
    {
        ServiceLocator.instance.GetService<PPStorage>().playerProperties.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnablePlayerJoining()
    {
        playerInputManager.EnableJoining();
    }

    public void DisablePlayerJoining()
    {
        playerInputManager.DisableJoining();
    }

    //public void SelectionOfArena(string name)
    //{
    //    blackScreen.SetActive(true);
    //    menuButton.SetActive(false);
    //    blackScreen.GetComponent<Animator>().enabled = true;

    //    ServiceLocator.instance.GetService<PPStorage>().SetArenaName(name);
    //    StartCoroutine(SceneTransition());
    //}

    //private IEnumerator SceneTransition()
    //{
    //    yield return new WaitForSeconds(3.0f);
    //    SceneManager.LoadScene(SceneName);
    //}
    
    //public void DisplayCreditScene()
    //{
    //    //creditSceneEventSystem.SetActive(true);
    //    startScreen.SetActive(false);
    //    additionalCreditScene.SetActive(false);
    //    creditScene.SetActive(true);
    //}

    //public void DisplayAdditionalCredits()
    //{
    //    creditScene.SetActive(false);
    //    additionalCreditScene.SetActive(true);
    //}

    //public void BackToStartScene()
    //{
    //    //creditSceneEventSystem.SetActive(false);
    //    creditScene.SetActive(false);
    //    additionalCreditScene.SetActive(false);
    //    startScreen.SetActive(true);
    //}

    //public void DisableBackground()
    //{
    //    crowdBackground.enabled = false;
    //}

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ReloadScene();
        }
    }
}
