using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonPortalController : Interactable
{
    [SerializeField] private string nextSceneName, scenePassword;
    private void Awake() {
      
    }
    public override void Interact(){
        PlayerController.instance.scenePassword = scenePassword;
        SceneManager.LoadScene(nextSceneName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TurnOnIIcon(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TurnOffIIcon(collision);
    }

}
