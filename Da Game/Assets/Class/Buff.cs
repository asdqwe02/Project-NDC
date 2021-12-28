using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Interactable
{

    private PlayerController pc;
    protected bool isTriggered = false;
    [SerializeField] BuffType buffType;
    [SerializeField] GameObject statStickPrefab; //This thing will never be instantiate 
    private PlayerClass statStick;
    Animator animator;

    public enum BuffType
    {
        PhysicalAttack,
        FireAttack,
        ColdAttack,
        LightningAttack,
        Armour,
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

   public void SetUp(BuffType buff)
    {
        buffType = buff;
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
                    pc.DamageType_ = MovingObjects.DamageType.Physical;
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
                case BuffType.Armour:
                    pc.Armour += statStick.Armour;
                    break;
                case BuffType.FireResistance:
                    pc.FireResistance +=statStick.FireResistance;
                    break;
                case BuffType.ColdResistance:
                    pc.ColdResistance += statStick.ColdResistance;
                    break;
                case BuffType.LightningResistance:
                    pc.LightningResistance += statStick.LightningResistance;
                    break;
                case BuffType.HPBoost:
                    pc.MaxHP += statStick.Hp;
                    pc.Hp += statStick.Hp;
                    break;
                case BuffType.SingleBullet:
                    pc.FireType = 0;
                    pc.Damage = pc.BaseDamage; //reset damage to base damage
                    pc.FireRate = statStick.FireRate;
                    break;
                case BuffType.MultiBullet:
                    pc.FireType = 1;
                    pc.Damage = pc.BaseDamage / 2; // reduce damage for spread mode 
                    pc.FireRate = statStick.FireRate * 10;
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
        int temp = (int)buffType;
        switch (temp)
        {
            case 0:
                animator.SetBool("isPhysAtt", true);
                break;
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
                animator.SetBool("isArmour", true);
                break;
            case 5:
                animator.SetBool("isFireRes", true);
                break;
            case 6:
                animator.SetBool("isColdRes", true);
                break;
            case 7:
                animator.SetBool("isLightningRes", true);
                break;
            case 8:
                animator.SetBool("isHPBoost", true);
                break;
            case 9:
                animator.SetBool("isSingleBullet", true);
                break;
            case 10:
                animator.SetBool("isMultiBullet", true);
                break;
            default:
                break;
        }
    }
}
