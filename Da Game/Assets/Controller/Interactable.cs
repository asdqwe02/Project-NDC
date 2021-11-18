using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected bool isTriggered = false;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.pc = collision.GetComponent<PlayerController>();
            TurnOnIIcon(collision);
            isTriggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            this.pc = null;
            TurnOffIIcon(collision);
            isTriggered = false;
            return;
        }
    }

    public void turnoffIsTrigger()
    {
        isTriggered = false;
    }
}
