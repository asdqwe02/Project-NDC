using UnityEngine;
using Pathfinding;
public class MovingObjects : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int armour;
    [SerializeField] protected float Damage;


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

    public void takeDamage(float damage)
    {
        hp -= damage;
    }



}
