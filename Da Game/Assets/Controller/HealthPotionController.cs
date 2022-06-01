using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionController : Interactable
{
    PlayerController pc; // Might need to use this in multiplayer mode if it's ever being implemented
    public float HealHP=0;
    public Transform numberPopUp;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // pc = collision.gameObject.GetComponent<PlayerController>();
        if (collision.CompareTag("Player"))
        {
            Interact();
        }
    }
    public override void Interact()
    {
        HealHP = PlayerController.instance.MaxHP * 0.2f;
        PlayerController.instance.Hp += HealHP;
        if (PlayerController.instance.Hp > PlayerController.instance.MaxHP)
            PlayerController.instance.Hp = PlayerController.instance.MaxHP;
        numberPopUp.GetComponent<NumberPopupController>().HealingNumberSetUp(HealHP);
        Instantiate(numberPopUp, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
