using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
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
