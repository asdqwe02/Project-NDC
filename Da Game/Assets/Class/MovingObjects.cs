using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingObjects : MonoBehaviour
{
    private float HP;
    private int MovementSpeed;
    private int AttackSpeed;
    private int Armour;

    public MovingObjects()
    {
        HP = 1;
        MovementSpeed = 1;
        AttackSpeed = 1;
        Armour = 1;
    }
    public MovingObjects(float Hp, int Ms, int As, int Arm)
    {
        HP = Hp;
        MovementSpeed = Ms;
        AttackSpeed = As;
        Armour = Arm;
    }
    void Move()
    {

    }
    void Die()
    {

    }
    void Shoot()
    {

    }
    void GetHit()
    {

    }
    void Idle()
    {

    }


}
