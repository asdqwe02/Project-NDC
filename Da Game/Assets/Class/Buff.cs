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
        Movespeed,
        NULL //NULL as last value (use this to see if there are any errors/bugs)
    }
    public struct BuffRNG
    {
        public Buff.BuffType buffType;
        public float weight;

        public BuffRNG(Buff.BuffType buffType, float weight)
        {
            this.buffType = buffType;
            this.weight = weight;
        }
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
    public BuffType GetBuff()
    {
        return buffType;
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

    //might want to change pc to PlayerController.instance and remove pc entirely
    public override void Interact() //need to improve the single and multiple fire buff a bit 
    {
        if (pc != null)
        {
            // increased damage by 10 % if fire type is the same as buffs
            float damageIncrease = pc.BaseDamage * 0.1f;
            if (damageIncrease > 2)
                damageIncrease = 2f;
            //increased damage by 10% if player damage type is the same as buff
            if ((int)pc.DamageType_ == (int)buffType)
            {
                pc.Damage += damageIncrease;
                pc.BaseDamage += damageIncrease;
            }
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
                    pc.FireResistance += statStick.FireResistance;
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

                    if (pc.FireType == 0)
                        pc.BaseDamage += damageIncrease;
                    else pc.FireType = 0;
                    pc.Damage = pc.BaseDamage; //reset damage to base damage
                    pc.FireRate = statStick.FireRate;
                    break;
                case BuffType.MultiBullet:
                    if (pc.FireType == 1)
                    {
                        pc.BaseDamage += damageIncrease;
                    }
                    else pc.FireType = 1;
                    pc.Damage = pc.BaseDamage / 2; // reduce damage for spread mode 
                    pc.FireRate = statStick.FireRate * 10;
                    break;
                case BuffType.Movespeed:
                    pc.MovementSpeed += statStick.MovementSpeed;
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
        //throw new System.NotImplementedException();
    }
    public static void ApplyBuff(BuffType buff, PlayerClass StatStick) //need to improve the single and multiple fire buff a bit 
    {
        float damageIncrease = PlayerController.instance.BaseDamage * 0.1f;
        if (damageIncrease > 2)
            damageIncrease = 2f;
        if ((int)PlayerController.instance.DamageType_ == (int)buff)
        {
            PlayerController.instance.Damage += damageIncrease;
            PlayerController.instance.BaseDamage += damageIncrease;
        }
        switch (buff)
        {
            case BuffType.PhysicalAttack:
                PlayerController.instance.DamageType_ = MovingObjects.DamageType.Physical;
                break;
            case BuffType.FireAttack:
                PlayerController.instance.DamageType_ = MovingObjects.DamageType.Fire;
                break;
            case BuffType.ColdAttack:
                PlayerController.instance.DamageType_ = MovingObjects.DamageType.Cold;
                break;
            case BuffType.LightningAttack:
                PlayerController.instance.DamageType_ = MovingObjects.DamageType.Lightning;
                break;
            case BuffType.Armour:
                PlayerController.instance.Armour += StatStick.Armour;
                break;
            case BuffType.FireResistance:
                PlayerController.instance.FireResistance += StatStick.FireResistance;
                break;
            case BuffType.ColdResistance:
                PlayerController.instance.ColdResistance += StatStick.ColdResistance;
                break;
            case BuffType.LightningResistance:
                PlayerController.instance.LightningResistance += StatStick.LightningResistance;
                break;
            case BuffType.HPBoost:
                PlayerController.instance.MaxHP += StatStick.Hp;
                PlayerController.instance.Hp += StatStick.Hp;
                break;
            case BuffType.SingleBullet:
                if (PlayerController.instance.FireType==0)
                    PlayerController.instance.BaseDamage += damageIncrease;
                else PlayerController.instance.FireType = 0;
                PlayerController.instance.Damage = PlayerController.instance.BaseDamage; //reset damage to base damage
                PlayerController.instance.FireRate = StatStick.FireRate;
                break;
            case BuffType.MultiBullet:
                if (PlayerController.instance.FireType == 1)
                    PlayerController.instance.BaseDamage += damageIncrease;
                else PlayerController.instance.FireType = 1;
                PlayerController.instance.Damage = PlayerController.instance.BaseDamage / 2; // reduce damage for spread mode 
                PlayerController.instance.FireRate = StatStick.FireRate * 10;
                break;
            case BuffType.Movespeed:
                PlayerController.instance.MovementSpeed += StatStick.MovementSpeed;
                break;
            default:
                break;
        }
    }
    private void ChangeAniToBuffType()
    {
        int temp = (int)buffType;
        switch (buffType)
        {
            case BuffType.PhysicalAttack:
                animator.SetBool("isPhysAtt", true);
                break;
            case BuffType.FireAttack:
                animator.SetBool("isFireAtt", true);
                break;
            case BuffType.ColdAttack:
                animator.SetBool("isColdAtt", true);
                break;
            case BuffType.LightningAttack:
                animator.SetBool("isLightningAtt", true);
                break;
            case BuffType.Armour:
                animator.SetBool("isArmour", true);
                break;
            case BuffType.FireResistance:
                animator.SetBool("isFireRes", true);
                break;
            case BuffType.ColdResistance:
                animator.SetBool("isColdRes", true);
                break;
            case BuffType.LightningResistance:
                animator.SetBool("isLightningRes", true);
                break;
            case BuffType.HPBoost:
                animator.SetBool("isHPBoost", true);
                break;
            case BuffType.SingleBullet:
                animator.SetBool("isSingleBullet", true);
                break;
            case BuffType.MultiBullet:
                animator.SetBool("isMultiBullet", true);
                break;
            case BuffType.Movespeed:
                animator.SetBool("isMovespeed", true);
                break;
            default:
                break;
        }
    }

    //Utility function for BuffRNG

    //using insertion sort to sort BuffRNG by weight
    public static void SortBuffRNGByWeight(ref BuffRNG[] buffs)
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            for (int j = i + 1; j < buffs.Length; j++)
            {
                if (buffs[i].weight > buffs[j].weight)
                    SwapBuffRNG(ref buffs[i], ref buffs[j]);
            }
        }
    }
    public static void SwapBuffRNG(ref BuffRNG a, ref BuffRNG b)
    {
        BuffRNG temp = a;
        a = b;
        b = temp;
    }
    public static float BuffRNGTotalWeight(BuffRNG[] buffWeight)
    {
        float sum = 0f;
        foreach (BuffRNG item in buffWeight)
        {
            sum += item.weight;
        }
        return sum;
    }
    public static BuffType RollBuffRNG(BuffRNG[] buffs)
    {
        float CumulativeProbability = 0f;
        float RNGRoll = Random.Range(0f, 1f);
        float TotalWeight = BuffRNGTotalWeight(buffs);
        foreach (BuffRNG item in buffs)
        {
            CumulativeProbability += item.weight / TotalWeight;
            if (RNGRoll <= CumulativeProbability)
                return item.buffType;
        }
        return BuffType.NULL; // this should never happened if it's null it's a bug 
    }

    //second option for rolling BuffRNG
    public static BuffType RollBuffRNG(BuffRNG[] buffs, float RNGRoll)
    {
        float CumulativeProbability = 0f;
        float TotalWeight = BuffRNGTotalWeight(buffs);
        foreach (BuffRNG item in buffs)
        {
            CumulativeProbability += item.weight / TotalWeight;
            if (RNGRoll <= CumulativeProbability)
                return item.buffType;
        }
        return BuffType.NULL; // this should never happened if it's null it's a bug 
    }
    public static void RemoveDamageTypeBuffsFromPool(ref BuffRNG[] buffs)
    {
        List<BuffRNG> temp = new List<BuffRNG>();
        foreach (BuffRNG item in buffs)
        {
            switch (item.buffType)
            {
                case BuffType.PhysicalAttack:
                case BuffType.FireAttack:
                case BuffType.ColdAttack:
                case BuffType.LightningAttack:
                    break;
                default:
                    BuffRNG tempBuffRNG = new BuffRNG(item.buffType, item.weight);
                    temp.Add(tempBuffRNG);
                    break;
            }
        }
        //System.Array.Clear(buffs, 0, buffs.Length);
        buffs = temp.ToArray(); // might be bad in term of memory managing 
    }
}
