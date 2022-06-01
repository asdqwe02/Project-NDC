using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStageController : Interactable
{
    public GameObject ToDisplay;
    Collider2D colliderP;
    [SerializeField] private string newScenePassword;
    private void Awake() {
        if (ToDisplay == null)
            ToDisplay = GameObject.Find("Boss Room Canvas").transform.Find("EndMessage").gameObject;
    }
    public override void Interact()
    {
        PlayerController.instance.CloseInteractableIcon();
        PlayerController.instance.scenePassword = newScenePassword;
        PlayerController.instance.Save();
        ToDisplay.SetActive(true);
        Time.timeScale = 0f;
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
