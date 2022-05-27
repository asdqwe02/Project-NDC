using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNG 
{
    public float chance, roll;
    public void SetUp(float Chance) //DUMB TODO: REMOVE
    {
        chance = Chance;
    }
    public bool RollNumber(float Chance)
    {
        chance = Chance;
        roll = Random.Range(0, 101);
        if (roll <= chance)
            return true;
        else return false;
    }

   
}
