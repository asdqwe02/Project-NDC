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

    [Header("Basic Stats")]
    [SerializeField] private float hp;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int armour;
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;
    [SerializeField] protected List<StatusEffect> statusEffects = new List<StatusEffect>();

    public float Hp { get => hp; set => hp = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Armour { get => armour; set => armour = value; }
    public float Damage { get => damage; set => damage = value; }
    public DamageType DamageType_ { get => damageType; set => damageType = value; }

    public MovingObjects()
    {
        hp = 1;
        movementSpeed = 1;
        attackSpeed = 1;
        armour = 1;
        Damage = 1;
        
    }
    public MovingObjects(float Hp, float Ms, float As, int Armor, float damage )
    {
        hp = Hp;
        movementSpeed = Ms;
        attackSpeed = As;
        armour = Armor;
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
    public void ApplyStatusEffect(int damagetype)
    {
        if (statusEffects.Count >= 3)
            return;
        StatusEffect tempStatusEffect = (StatusEffect) damagetype;

        if (statusEffects.Contains(tempStatusEffect))
            return;
        else
        {
            statusEffects.Add(tempStatusEffect);
            //update player's gfx status bar
            if (IsPlayer)
            {
                GameObject temp;
                GameObject parent = GameObject.Find("Status Bar");
                switch (damagetype)
                {
                    case 1:
                        temp = Instantiate(Resources.Load("StatusEffectGFX/BurntEffectGFX") as GameObject);
                        temp.transform.SetParent(parent.transform);
                        break;
                    case 2:

                        temp = Instantiate(Resources.Load("StatusEffectGFX/FrozenEffectGFX") as GameObject);
                        temp.transform.SetParent(parent.transform);
                        break;
                    case 3:
                        temp = Instantiate(Resources.Load("StatusEffectGFX/ShockedEffectGFX") as GameObject);
                        temp.transform.SetParent(parent.transform);
                        break;

                }
                    


            }

            switch (damagetype)
            {
                case 1:
                    InvokeRepeating("BurningTimer", 0f, Time.fixedDeltaTime);
                    break;
                case 2:
                    InvokeRepeating("FreezingTimer", 0f, Time.fixedDeltaTime);
                    break;
                case 3:
                    InvokeRepeating("ShockedTimer", 0f, Time.fixedDeltaTime);
                    break;
                default:
                    break;
            }
        }
    }
    public void RemoveStatusEffect(int statustype)
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
                    Destroy(GameObject.Find("BurntEffectGFX(Clone)"));
                    break;
                case 2:
                    Destroy(GameObject.Find("FrozenEffectGFX(Clone)"));

                    break;
                case 3:
                    Destroy(GameObject.Find("ShockedEffectGFX(Clone)"));
                    break;

            }



        }

    }
    public void takeDamage(float damage)
    {
        hp -= damage;
    }
    private void BurningTimer()
    {
        burningTimer -= Time.fixedDeltaTime;
        if (burningTimer <= 0)
        {
            burningTimer = 3f;
            RemoveStatusEffect(1);
            CancelInvoke("BurningTimer");
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

    protected void DropMoney(int low, int high)
    {
        if (!dropped)
        {
            PlayerController.instance.Money += Random.RandomRange(1, 10);
            dropped = true;
        }
    }
}
