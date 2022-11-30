using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
   
    protected PlayerController pc; // Might need to use this in multiplayer mode if it's ever being implemented
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
