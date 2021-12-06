using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMovingObject : MonoBehaviour
{
    public float hp;
    public void takdeDamage(float damage)
    {
        hp -= damage;
        if (checkDestroy())
            Destroy(gameObject, 4f);
    }
    public bool checkDestroy()
    {
        if (hp <= 0)
            return true;
        return false;
    }
}
