using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject PauseMenu_Confirmation;
    public GameObject DeathMenu;
    private GameObject mainCanvas;
    public GameObject HO_PauseMenu;
    public static MenuController instance;

    private void Awake() 
    {
        if (instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }
        mainCanvas = GameObject.Find("Canvas").gameObject;
        if (PauseMenu == null)
        {
            PauseMenu = mainCanvas.transform.Find("Pause Menu").gameObject;
            PauseMenu_Confirmation = PauseMenu.transform.Find("Retreat confirm").gameObject;
            DeathMenu = mainCanvas.transform.Find("DeathMenu").gameObject;
        }
        if (SceneManager.GetActiveScene().name=="Hideout")
            LoadHideoutPauseMenu();
        SceneManager.sceneLoaded += ReloadMenuObject;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Hideout" && HO_PauseMenu==null)
        {
            LoadHideoutPauseMenu();
        }
        CheckInput();
        if(!PlayerController.instance.InHO) // let player controller trigger this
            CheckPlayerDeath();
    }
    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (SceneManager.GetActiveScene().name == "Hideout" || SceneManager.GetActiveScene().name == "Tutorial")
            {
                if (HO_PauseMenu.activeSelf)
                {
                    Scene currentScene = SceneManager.GetActiveScene();
                    string sceneName = currentScene.name;
                    if (sceneName!="Hideout" && sceneName != "Tutorial"  && PauseMenu_Confirmation.activeSelf)
                        PauseMenu_Confirmation.SetActive(false);
                    HO_PauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    HO_PauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
            else {
                if (PauseMenu.activeSelf)
                {
                    Scene currentScene = SceneManager.GetActiveScene();
                    string sceneName = currentScene.name;
                    if (sceneName!="Hideout" && sceneName != "Tutorial"  && PauseMenu_Confirmation.active)
                        PauseMenu_Confirmation.SetActive(false);
                    PauseMenu.SetActive(false);
                    Time.timeScale = 1f;


                }
                else
                {
                    PauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
           
        }
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
        if (MainCanvasController.instance!=null)
            MainCanvasController.instance.DestroySelf();
        SceneManager.LoadScene("MainMenu");
        // mainCanvas.SetActive(false);
        Time.timeScale = 1f;
        PlayerController.instance.gameObject.SetActive(false);
    }

    public void BackToHideout()
    {
        PlayerController.instance.Save();
        PlayerController.instance.scenePassword = "Hideout";
        // mainCanvas.SetActive(true);
        PlayerController.instance.Load_Base();
        if (MainCanvasController.instance!=null)
            MainCanvasController.instance.DestroySelf();
        SceneManager.LoadScene("Hideout");
        Time.timeScale = 1f;
    }

    public void BackToHideout_Penalty()
    {
        PlayerController.instance.coins -= (int)(PlayerController.instance.Coin_tobeAdded * 0.7f);
        PlayerController.instance.Save();
        PlayerController.instance.scenePassword = "Hideout";
        PlayerController.instance.Load_Base();
        if (MainCanvasController.instance!=null)
            MainCanvasController.instance.DestroySelf();
        SceneManager.LoadScene("Hideout");

        Time.timeScale = 1f;
    }

    public void BackToHideout_Death()
    {
        PlayerController.instance.Death = false;
        PlayerController.instance.animator.SetBool("IsDying", false);
        PlayerController.instance.coins -= (int)(PlayerController.instance.Coin_tobeAdded * 1);
        PlayerController.instance.scenePassword = "Hideout";
        PlayerController.instance.Load_Base();
        if (MainCanvasController.instance!=null)
            MainCanvasController.instance.DestroySelf();
        SceneManager.LoadScene("Hideout");
        Time.timeScale = 1f;
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
    public void ReloadMenuObject(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Hideout")
        {
            mainCanvas = GameObject.Find("Canvas").gameObject;
            PauseMenu = mainCanvas.transform.Find("Pause Menu").gameObject;
            DeathMenu = mainCanvas.transform.Find("DeathMenu").gameObject;

            PauseMenu.transform.Find("Retreat").GetComponent<Button>().onClick.AddListener(()=>{
                PauseMenu.transform.Find("Retreat confirm").gameObject.SetActive(true);
            });
            PauseMenu_Confirmation = PauseMenu.transform.Find("Retreat confirm").gameObject;
            PauseMenu_Confirmation.transform.Find("Yes button").GetComponent<Button>().onClick.AddListener(()=>{
                BackToHideout_Penalty();
            });
            PauseMenu_Confirmation.transform.Find("No button").GetComponent<Button>().onClick.AddListener(()=>{
                PauseMenu.SetActive(false);
                PauseMenu_Confirmation.SetActive(false);
                UnfreezeTime();
            });
            DeathMenu = mainCanvas.transform.Find("DeathMenu").gameObject;
            InventoryController.instance.ReloadInventoryUI();
            GetComponent<StatSheetController>().ReloadStatSheetWindow();
        }
    }
    public void LoadHideoutPauseMenu()
    {
        HO_PauseMenu = GameObject.Find("HO_Canvas").transform.Find("Pause Menu").gameObject;
        HO_PauseMenu.transform.Find("Exit Main Menu").GetComponent<Button>().onClick.AddListener(() => BackToMainMenu());
    }
}
