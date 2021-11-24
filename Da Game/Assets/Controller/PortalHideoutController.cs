using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PortalHideoutController : Interactable
{

    public string sceneName;
    Collider2D colliderP;
    [SerializeField] private string newScenePassword;
    public override void Interact()
    {
        PlayerController.instance.CloseInteractableIcon();
        PlayerController.instance.scenePassword = newScenePassword;
        PlayerController.instance.Save();
        SceneManager.LoadScene(sceneName);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        TurnOnIIcon(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliderP = null;
        TurnOffIIcon(collision);
    }
}
