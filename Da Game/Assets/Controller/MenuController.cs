using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject DeathMenu;

    private void Start()
    {
        
    }
    private void Update()
    {
        CheckInput();
        if(!PlayerController.instance.InHO)
            CheckPlayerDeath();
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.active)
            {
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

    public void CheckPlayerDeath()
    {
        if (PlayerController.instance.Death == true)
        {
            //PlayerController.instance.CleanseEffect();
            DeathMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        //if (PlayerController.instance.hp <= 0)
        //{
        //    PlayerController.instance.animator.SetBool("IsDying", true);
            
        //}
        //else
        //{
        //    PlayerController.instance.animator.SetBool("IsDying", false);
        //}
    }

    public void BackToMainMenu()
    {
        PlayerController.instance.Save();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void BackToHideout()
    {
        PlayerController.instance.Save();
        PlayerController.instance.scenePassword = "Hideout";
        PlayerController.instance.Load_Base();
        SceneManager.LoadScene("Hideout");
        Time.timeScale = 1f;
    }

    public void BackToHideout_Penalty()
    {
        PlayerController.instance.coins -= (int)(PlayerController.instance.Coin_tobeAdded * 0.7);
        PlayerController.instance.Save();
        PlayerController.instance.scenePassword = "Hideout";
        PlayerController.instance.Load_Base();
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
        SceneManager.LoadScene("Hideout");
        Time.timeScale = 1f;
    }

    //this function shouldn't be here but I don't want to make a whole new script to do this one simple function
    public void TutorialSceneReset() 
    {
        PlayerController.instance.Load_Base();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
