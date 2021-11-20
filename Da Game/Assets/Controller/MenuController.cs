using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject PauseMenu;

    private void Start()
    {
        
    }
    private void Update()
    {
        CheckInput();
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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void BackToHideout()
    {
        PlayerController.instance.scenePassword = "Hideout";
        SceneManager.LoadScene("Hideout");
        Time.timeScale = 1f;
    }

}
