using UnityEngine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
public class MovingObjects : MonoBehaviour
{   
    [Header("Status Effect Timer")]
    [SerializeField] protected float shockedTimer = 4f;
    [SerializeField] protected float burningTimer = 3f;
    [SerializeField] protected float freezingTimer = 2f;

    [Header("Basic Stats")]
    [SerializeField] protected float hp;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int armour;
    [SerializeField] protected float Damage;
    [SerializeField] protected DamageType damageType;
    [SerializeField] protected List<StatusEffect> statusEffects = new List<StatusEffect>();
  
   


    public MovingObjects()
    {
        hp = 1;
        movementSpeed = 1;
        attackSpeed = 1;
        armour = 1;
        Damage = 1;
    }
    public MovingObjects(float Hp, float Ms, float As, int Armor, float damage)
    {
        hp = Hp;
        movementSpeed = Ms;
        attackSpeed = As;
        armour = Armor;
        Damage = damage;
    }
    protected enum DamageType
    {
        Physical,
        Fire,
        Cold,
        Lightning
        
    }
    protected enum StatusEffect
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
}
