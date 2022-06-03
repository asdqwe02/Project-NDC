using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndStageController : Interactable
{
    public GameObject ToDisplay;
    Collider2D colliderP;
    [SerializeField]private GameObject bossEnemy;
    CircleCollider2D collider;
    SpriteRenderer spriteRenderer;

    [SerializeField] private string newScenePassword;
    private void Awake() {
        bossEnemy = GameObject.FindGameObjectWithTag("Enemy");
        collider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DisablePortal();
        if (ToDisplay == null)
            ToDisplay = GameObject.Find("Boss Room Canvas").transform.Find("EndMessage").gameObject;
        ToDisplay.transform.Find("Retreat").GetComponent<Button>().onClick.AddListener(()=>{
            MenuController.instance.BackToHideout();
        });
        
    }
    private void FixedUpdate() {
        if (bossEnemy == null)
            EnablePortal();
    }
    public override void Interact()
    {
        PlayerController.instance.CloseInteractableIcon();
        PlayerController.instance.scenePassword = newScenePassword;
        PlayerController.instance.Save();
        ToDisplay.SetActive(true);
        Time.timeScale = 0f;
    }
    public void DisablePortal()
    {
        collider.enabled =false;
        spriteRenderer.enabled= false;
    }
    public void EnablePortal()
    {
        collider.enabled =true;
        spriteRenderer.enabled= true;
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
