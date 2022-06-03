using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    //currently 12 buffs if increased or reduce amount of buffs in pool please change the number
    public static Buff.BuffRNG[] BasicBuffsPool = new Buff.BuffRNG[12]
    {
         new Buff.BuffRNG(Buff.BuffType.PhysicalAttack,250),
         new Buff.BuffRNG(Buff.BuffType.FireAttack,250),
         new Buff.BuffRNG(Buff.BuffType.ColdAttack,250),
         new Buff.BuffRNG(Buff.BuffType.LightningAttack,250),
         new Buff.BuffRNG(Buff.BuffType.Armour,300),
         new Buff.BuffRNG(Buff.BuffType.FireResistance,300),
         new Buff.BuffRNG(Buff.BuffType.ColdResistance,300),
         new Buff.BuffRNG(Buff.BuffType.LightningResistance,300),
         new Buff.BuffRNG(Buff.BuffType.HPBoost,500),
         new Buff.BuffRNG(Buff.BuffType.SingleBullet,150),
         new Buff.BuffRNG(Buff.BuffType.MultiBullet,150),
         new Buff.BuffRNG(Buff.BuffType.Movespeed,400)
    };
    public static ModifierPool WeaponModifierPool = new ModifierPool(type:"weapon", jsonFilePath:"/StreamingAssets/ItemModifiers.json");
    public static ModifierPool ArmourModifierPool = new ModifierPool(type:"armour", jsonFilePath:"/StreamingAssets/ItemModifiers.json");

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
    public static float GetAngleFromVectorFloatWF(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return n;
    }
    public static Vector3 GetRandomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public static Vector2 RotateA2DVector(Vector2 vector, float angle) //doesn't work correctly
    {
        //x2=cosβx1−sinβy1
        //y2=sinβx1+cosβy1
        float newX = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        float newY = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);
        return new Vector2(newX, newY);
    }
    public static bool IsAnimationPlaying(Animator anim, string stateName) // only work for layer 0 animation
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
