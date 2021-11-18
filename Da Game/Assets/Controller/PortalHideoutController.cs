using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PortalHideoutController : Interactable
{

    public string sceneName;
    [SerializeField] private string newScenePassword;
    public override void Interact()
    {
        PlayerController.Singleton.CloseInteractableIcon();
        isTriggered = false;
        turnoffIsTrigger();
        PlayerController.instance.scenePassword = newScenePassword;
        PlayerController.instance.Save();
        SceneManager.LoadScene(sceneName);

    }
}
