using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Interactable
{
    private bool isTriggered = false;
    private PlayerController pc;
    [SerializeField] BuffType buffType;
    [SerializeField] GameObject statStickPrefab; //This thing will never be instantiate 
    private PlayerClass statStick;
    Animator animator;

    enum BuffType
    {
        PhysicalAttack,
        FireAttack,
        ColdAttack,
        LightningAttack,
        FireResistance,
        ColdResistance,
        LightningResistance,
        HPBoost,
        SingleBullet,
        MultiBullet,
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        statStick = statStickPrefab.GetComponent<PlayerClass>();
    }
    private void Update()
    {
        if (isTriggered && Input.GetKeyDown(KeyCode.E) && pc != null)
        {
            Interact();
        }
    }

    //delete later
    public string getBuffType()
    {
        return null;
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
    public override void Interact()
    {
        if (pc != null) 
        { 
            switch (buffType)
            {
                case BuffType.PhysicalAttack:
                    break;
                case BuffType.FireAttack:
                    pc.DamageType_ = MovingObjects.DamageType.Fire;
                    break;
                case BuffType.ColdAttack:
                    pc.DamageType_ = MovingObjects.DamageType.Cold;
                    break;
                case BuffType.LightningAttack:
                    pc.DamageType_ = MovingObjects.DamageType.Lightning;
                    break;
                case BuffType.FireResistance:
                    break;
                case BuffType.ColdResistance:
                    break;
                case BuffType.LightningResistance:
                    break;
                case BuffType.HPBoost:
                    pc.Hp += statStick.Hp;
                    break;
                case BuffType.SingleBullet:
                    pc.FireType = 0;
                    pc.FireRate = statStick.FireRate;
                    break;
                case BuffType.MultiBullet:
                    pc.FireType = 1;
                    pc.FireRate = statStick.FireRate*10;
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
        //throw new System.NotImplementedException();
    }
   
    private void ChangeAniToBuffType()
    {
        int temp = (int) buffType;
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
            case 7:
                animator.SetBool("isHPBoost", true);
                break;
            case 8:
                animator.SetBool("isSingleBullet", true);
                break;
            case 9:
                animator.SetBool("isMultiBullet", true);
                break;
            default:
                break;
        }
    }
}
