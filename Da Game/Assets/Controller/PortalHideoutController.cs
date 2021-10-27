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
        PlayerController.instance.scenePassword = newScenePassword;
        SceneManager.LoadScene(sceneName);
    }
}
