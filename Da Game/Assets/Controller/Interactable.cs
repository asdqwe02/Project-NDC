using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
   
    private PlayerController pc;
    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger= true;
    }
    public abstract void Interact();

    public void TurnOnIIcon(Collider2D collideP)
    {
        if (collideP.gameObject.CompareTag("Player"))
        {
            collideP.GetComponent<PlayerController>().OpenInteractableIcon();
        }

    }
    public void TurnOffIIcon(Collider2D collideP)
    {
        if (collideP.gameObject.CompareTag("Player"))
        {
            collideP.GetComponent<PlayerController>().CloseInteractableIcon();
        }
    }
 
    //public void turnoffIsTrigger()
    //{
    //    isTriggered = false;
    //}
}
