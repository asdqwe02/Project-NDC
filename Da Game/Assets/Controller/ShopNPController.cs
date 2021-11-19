using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPController : Interactable
{
    public GameObject shop;

    public override void Interact()
    {
        shop.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TurnOnIIcon(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TurnOffIIcon(collision);
        shop.SetActive(false);
    }
}
