using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private string _buffType = "";
    [SerializeField] private int _speedInc = 0;
    private bool isTriggered = false;
    private PlayerController pc;
    [SerializeField] BuffType buffType;
    Animator animator;

    enum BuffType
    {
        PhysicalAttack,
        FireAttack,
        ColdAttack,
        LightningAttack,
        FireResistance,
        ColdResistance,
        LightningResistance
    }
    private void Start()
    {
         animator = GetComponent<Animator>();
    }
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
    private void ChangeToBuffType()
    {
        int temp = (int)buffType;
        Debug.Log("buff type: " + temp);
        switch (temp)
        {
            case 1:
                animator.SetBool("isFireAtt", true);
                break;
            case 2:
                animator.SetBool("isColdAtt", true);
                break;
            case 3:
                animator.SetBool("isLightningAtt", true);
                break;
            case 4:
                animator.SetBool("isFireRes", true);
                break;
            case 5:
                animator.SetBool("isColdRes", true);
                break;
            case 6:
                animator.SetBool("isLightningRes", true);
                break;
            default:
                break;
        }
    }
}
