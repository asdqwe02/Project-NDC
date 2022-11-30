using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionController : Interactable
{
    public float HealHP = 0;
    public Transform numberPopUp;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // pc = collision.gameObject.GetComponent<PlayerController>();
        if (collision.CompareTag("Player"))
        {
            pc = collision.GetComponent<PlayerController>();
            Interact();
        }
    }
    public override void Interact()
    {
        HealHP = PlayerController.instance.MaxHP * 0.2f;
        pc.Hp += HealHP;
        if (pc?.Hp > pc?.MaxHP)
            pc.Hp = pc.MaxHP;
        numberPopUp.GetComponent<NumberPopupController>().HealingNumberSetUp(HealHP);
        Instantiate(numberPopUp, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
