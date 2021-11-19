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
            if(PauseMenu.active)
                PauseMenu.SetActive(false);
            else
                PauseMenu.SetActive(true);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToHideout()
    {
        PlayerController.instance.scenePassword = "Hideout";
        SceneManager.LoadScene("Hideout");
    }

}
