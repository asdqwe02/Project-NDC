using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class MenuController : MonoBehaviour
{
    [SerializeField] bool destroyOnLoad;

    public GameObject PauseMenu;
    public GameObject PauseMenu_Confirmation;
    public GameObject DeathMenu;
    private GameObject mainCanvas;
    public GameObject HO_PauseMenu;
    public static MenuController instance;
    private Material _pauseScreenMaterial;
    private Action homeButtomAction;
    public EventHandler OnSceneChangeEvent;
    [Header("Pause Screen GameObjects")]
    [SerializeField] GameObject pauseScreenCanvas;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject returnToHideout;
    [SerializeField] Button returnHideOutConfirm;
    [SerializeField] Button returnHideOutDeny;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        _pauseScreenMaterial = pauseScreen?.GetComponent<Image>().material;
        mainCanvas = GameObject.Find("Canvas").gameObject;
        homeButtomAction = () =>
        {
            BackToMainMenu();
            Debug.Log("test home button press");
            // returnToHideout.SetActive(true);
        };
        returnHideOutConfirm.onClick.AddListener(homeButtomAction.Invoke);
        // if (SceneManager.GetActiveScene().name == "Hideout")
        //     LoadHideoutPauseMenu();
        if (!destroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += ReloadMenuObjects;
        InventoryController.instance.ReloadInventoryUI();

    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Hideout" && HO_PauseMenu == null)
        {
            LoadHideoutPauseMenu();
        }
        CheckInput();
        if (!PlayerController.instance.InHO) // let player controller trigger this
            CheckPlayerDeath();
    }
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScreen?.SetActive(!pauseScreen.activeSelf);
            if (pauseScreen.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else Time.timeScale = 1f;

        }
        if (pauseScreen.activeSelf)
            _pauseScreenMaterial.SetFloat("_UnscaledTime", Time.unscaledTime); // explain: this make the shader effect not depend on scaled time 
    }

    public void CheckPlayerDeath()
    {
        if (PlayerController.instance.Death == true)
        {
            //PlayerController.instance.CleanseEffect();
            DeathMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void BackToMainMenu()
    {
        PlayerController.instance.Save();
        if (MainCanvasController.instance != null)
            MainCanvasController.instance.DestroySelf();

        // Sound Track Switch
        AudioManager.Instance.PlaySoundTrack(AudioManager.SoundTrack.MainMenuST);
        SceneManager.LoadScene("MainMenu");
        // mainCanvas.SetActive(false);
        Time.timeScale = 1f;
        PlayerController.instance.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void BackToHideout()
    {
        PlayerController.instance.Save();
        PlayerController.instance.scenePassword = "Hideout";
        PlayerController.instance.Load_Base();
        if (MainCanvasController.instance != null)
            MainCanvasController.instance.DestroySelf();
        AudioManager.Instance.PlaySoundTrack(AudioManager.SoundTrack.HideoutST);
        SceneManager.LoadScene("Hideout");
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void BackToHideout_Penalty()
    {
        PlayerController.instance.coins -= (int)(PlayerController.instance.Coin_tobeAdded * 0.7f);
        Debug.Log(PlayerController.instance.coins + "coins");
        BackToHideout();
    }

    public void BackToHideout_Death()
    {
        PlayerController.instance.Death = false;
        PlayerController.instance.animator.SetBool("IsDying", false);
        PlayerController.instance.coins -= (int)(PlayerController.instance.Coin_tobeAdded * 1);
        PlayerController.instance.scenePassword = "Hideout";
        PlayerController.instance.Load_Base();
        if (MainCanvasController.instance != null)
            MainCanvasController.instance.DestroySelf();
        AudioManager.Instance.PlaySoundTrack(AudioManager.SoundTrack.HideoutST);
        SceneManager.LoadScene("Hideout");
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
    public void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }
    //this function shouldn't be here but I don't want to make a whole new script to do this one simple function
    public void TutorialSceneReset()
    {
        PlayerController.instance.Load_Base();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void ReloadMenuObjects(Scene scene, LoadSceneMode mode)
    {
        pauseScreen = GameObject.Find("Pause Screen Canvas").transform.GetChild(0).gameObject;
        _pauseScreenMaterial = pauseScreen?.GetComponent<Image>().material;
        returnToHideout = pauseScreen.GetComponent<PauseScreen>().retreatPanel;
        if (scene.name != "Hideout")
        {
            homeButtomAction = () =>
            {
                Debug.Log("test home button press");
                returnToHideout.SetActive(true);
            };
            pauseScreen.GetComponent<PauseScreen>().homeButton.onClick.AddListener(homeButtomAction.Invoke);
            returnHideOutConfirm = pauseScreen.GetComponent<PauseScreen>().returnHideOutConfirm;
            returnHideOutConfirm.onClick.AddListener(BackToHideout_Penalty);
        }
        if (scene.name == "Tutorial")
        {
            LoadHideoutPauseMenu();
            InventoryController.instance.ReloadInventoryUI();
            GetComponent<StatSheetController>().ReloadStatSheetWindow();
            GameObject.Find("HO_Canvas").transform.Find("Reset Tutorial Scene Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                TutorialSceneReset();
            });

        }
    }
    public void LoadHideoutPauseMenu()
    {
        HO_PauseMenu = GameObject.Find("HO_Canvas").transform.Find("Pause Menu").gameObject;
        HO_PauseMenu.transform.Find("Exit Main Menu").GetComponent<Button>().onClick.AddListener(() => BackToMainMenu());
    }
    public void LoadHideoutShopUI()
    {
        GetComponent<ShopOwnerController>().LoadShopUI();
    }
    public void OnHomeButtonClick()
    {
        homeButtomAction?.Invoke();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ReloadMenuObjects;

    }
}
