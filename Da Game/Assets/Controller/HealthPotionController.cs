using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionController : Interactable
{
    PlayerController pc;
    public float HealHP=0;
    public Transform numberPopUp;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            Interact();
        }
    }
    public override void Interact()
    {
        pc.Hp += HealHP;
        numberPopUp.GetComponent<NumberPopupController>().HealingNumberSetUp(HealHP);
        Instantiate(numberPopUp, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
