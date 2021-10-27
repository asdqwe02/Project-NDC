using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger= true;
    }
    public abstract void Interact();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))   
        {
            collision.transform.GetComponent<PlayerController>().OpenInteractableIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().CloseInteractableIcon();
        }
    }


}
