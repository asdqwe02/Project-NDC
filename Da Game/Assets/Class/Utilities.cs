using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static Buff.BuffRNG[] BasicBuffsPool = new Buff.BuffRNG[11]
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
         new Buff.BuffRNG(Buff.BuffType.MultiBullet,150)
    };
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

}
