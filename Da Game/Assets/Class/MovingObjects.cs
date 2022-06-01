using UnityEngine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class MovingObjects : MonoBehaviour
{
    private bool dropped = false;
    protected bool IsPlayer = false;


    [Header("Status Effect Timer")]
    [SerializeField] protected float shockedTimer = 4f;
    [SerializeField] protected float burningTimer = 3f;
    [SerializeField] protected float freezingTimer = 2f;
    [Header("Basic Prefab")]
    [SerializeField] public Transform numberPopUp;
    [Header("Basic Stats")]
    [SerializeField] private float hp;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int armour;
    [SerializeField] private int fireResistance;
    [SerializeField] private int coldResistance;
    [SerializeField] private int lightningResistance;
    [SerializeField] private float damage;
    private float burningDamage = 0;    // dodn't make this public or serialized it 
    [SerializeField] private DamageType damageType;
    [SerializeField] protected List<StatusEffect> statusEffects = new List<StatusEffect>();

    public float Hp { get => hp; set => hp = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Armour { get => armour; set => armour = value; }
    public float Damage { get => damage; set => damage = value; }
    public DamageType DamageType_ { get => damageType; set => damageType = value; }
    public int FireResistance { get => fireResistance; set => fireResistance = value; }
    public int ColdResistance { get => coldResistance; set => coldResistance = value; }
    public int LightningResistance { get => lightningResistance; set => lightningResistance = value; }

    public MovingObjects()
    {
        hp = 1;
        movementSpeed = 1;
        attackSpeed = 1;
        armour = 1;
        fireResistance = 0;
        coldResistance = 0;
        lightningResistance = 0;
        Damage = 1;

    }
    public MovingObjects(float Hp, float Ms, float As, int Armor, int FireRes, int ColdRes, int LightingRes, float damage)
    {
        hp = Hp;
        movementSpeed = Ms;
        attackSpeed = As;
        armour = Armor;
        fireResistance = FireRes;
        coldResistance = ColdRes;
        lightningResistance = LightingRes;
        Damage = damage;

    }
    public enum DamageType
    {
        Physical,
        Fire,
        Cold,
        Lightning

    }
    public enum StatusEffect
    {
        None,
        Burning,
        Freeze,
        Shocked
    }
    public void ApplyStatusEffect(float damageTaken, DamageType damagetype)
    {
        if (statusEffects.Count >= 3)
            return;
        StatusEffect tempStatusEffect = (StatusEffect)((int)damagetype); //look stupid

        if (statusEffects.Contains(tempStatusEffect))
            return;
        else
        {
            statusEffects.Add(tempStatusEffect);
            //update player's gfx status bar (seem a bit repetitive)
            if (IsPlayer)
            {
                GameObject temp;
                GameObject parent = GameObject.Find("Status Bar");
                switch (damagetype)
                {
                    case DamageType.Fire:
                        temp = Instantiate(Resources.Load("StatusEffectGFX/BurntEffectGFX") as GameObject);
                        temp.transform.SetParent(parent.transform);
                        break;
                    case DamageType.Cold:

                        temp = Instantiate(Resources.Load("StatusEffectGFX/FrozenEffectGFX") as GameObject);
                        temp.transform.SetParent(parent.transform);
                        break;
                    case DamageType.Lightning:
                        temp = Instantiate(Resources.Load("StatusEffectGFX/ShockedEffectGFX") as GameObject);
                        temp.transform.SetParent(parent.transform);
                        break;
                    default: //usually mean physical can change later
                        break;

                }

            }
            switch (damagetype)
            {
                case DamageType.Fire:
                    burningDamage = (damageTaken / 5) * (1 - (float)fireResistance * 20 / 100) - (float)armour * 10 / 100;
                    InvokeRepeating("BurningTimer", 0f, Time.fixedDeltaTime);
                    InvokeRepeating("TakeBurningDamage", 0f, 0.95f); // 0.95s interval so it can deal all damage during burning time
                    break;
                case DamageType.Cold:
                    InvokeRepeating("FreezingTimer", 0f, Time.fixedDeltaTime);
                    break;
                case DamageType.Lightning:
                    InvokeRepeating("ShockedTimer", 0f, Time.fixedDeltaTime);
                    break;
                default: //usually mean physical can change later
                    break;
            }
        }
    }

    public void RemoveStatusEffect(int statustype) //Might want to imrpove this
    {
        StatusEffect tempStatusEffect = (StatusEffect)statustype;
        int removeIndes = statusEffects.IndexOf(tempStatusEffect);
        statusEffects.RemoveAt(removeIndes);
        if (IsPlayer)
        {
            GameObject parent = GameObject.Find("Status Bar");
            switch (statustype)
            {
                case 1:
                    CancelInvoke("BurningTimer");
                    CancelInvoke("TakeBurningDamage");
                    Destroy(GameObject.Find("BurntEffectGFX(Clone)"));
                    break;
                case 2:
                    CancelInvoke("FreezingTimer");
                    Destroy(GameObject.Find("FrozenEffectGFX(Clone)"));

                    break;
                case 3:
                    CancelInvoke("ShockedTimer");
                    Destroy(GameObject.Find("ShockedEffectGFX(Clone)"));
                    break;

            }



        }

    }
    public virtual void takeDamage(float damageTaken, DamageType damageTypeTaken)
    {
        float finalDamage = damageTaken;

        //damage increased on shocked enemies or player
        if (statusEffects.Contains(StatusEffect.Shocked))
            finalDamage = finalDamage * 1.3f;

        switch (damageTypeTaken)
        {
            //Might wanna change all defense value to float instead of int 
            case DamageType.Physical:
                finalDamage = finalDamage - (float)armour;
                break;
            case DamageType.Fire:
                finalDamage = finalDamage * (1 - (float)fireResistance * 20 / 100) - (float)armour * 10 / 100; //can change to 0.2 and 0.1
                break;
            case DamageType.Cold:
                finalDamage = finalDamage * (1 - (float)coldResistance * 20 / 100) - (float)armour * 10 / 100;
                break;
            case DamageType.Lightning:
                finalDamage = finalDamage * (1 - (float)lightningResistance * 20 / 100) - (float)armour * 10 / 100;
                break;
            default:
                break;
        }
        finalDamage = Mathf.Round(finalDamage * 100f) / 100f; //round final damage to have only 2 numbers after decimal point
        if (finalDamage < 0)
            finalDamage = 0f;
        hp -= finalDamage;
        if (numberPopUp != null)
        {
            numberPopUp.GetComponent<NumberPopupController>().DamageNumberSetUp(finalDamage, damageTypeTaken);
            Instantiate(numberPopUp, transform.position, Quaternion.identity);
        }
    }
    public virtual void takeDamage(float damageTaken, DamageType damageTypeTaken, Vector2 KnockBack)
    {
        //idk if this will summon the devil and kill me or not but it should work ... I think
        takeDamage(damageTaken, damageTypeTaken);

        //GetComponent<Rigidbody2D>().AddForce(KnockBack);
        //GetComponent<Rigidbody2D>().velocity = KnockBack * 10f;

    }
    private void BurningTimer()
    {
        burningTimer -= Time.fixedDeltaTime;
        if (burningTimer <= 0)
        {
            burningTimer = 3f;
            RemoveStatusEffect(1);
            CancelInvoke("BurningTimer");
            CancelInvoke("TakeBurningDamage");
        }
    }
    private void FreezingTimer()
    {
        freezingTimer -= Time.fixedDeltaTime;
        if (freezingTimer <= 0)
        {
            freezingTimer = 2f;
            RemoveStatusEffect(2);
            CancelInvoke("FreezingTimer");
        }
    }
    private void ShockedTimer()
    {
        shockedTimer -= Time.fixedDeltaTime;
        if (shockedTimer <= 0)
        {
            shockedTimer = 4f;
            RemoveStatusEffect(3);
            CancelInvoke("ShockedTimer");
        }
    }
    private void TakeBurningDamage()
    {
        takeDamage(burningDamage, DamageType.Fire);
    }

}
