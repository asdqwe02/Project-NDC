using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private string _buffType = "";
    [SerializeField] private int _speedInc = 0;
    private bool isTriggered = false;
    private PlayerController pc;

    private void Update()
    {
        if (isTriggered && Input.GetKeyDown(KeyCode.E) && pc != null)
        {
            pc.setBuff(this);
            Destroy(gameObject);
            return;
        }
    }
    public string getBuffType()
    {
        return this._buffType;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.pc = collision.GetComponent<PlayerController>();
        if (pc != null)
        {
            isTriggered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        this.pc = collision.GetComponent<PlayerController>();
        if (pc != null)
        {
            isTriggered = false;
            return;
        }
    }
    public int getSpeedInc()
    {
        return _speedInc;
    }
     
}
