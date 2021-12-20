using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int UnlockedSlots;
    public int coins,FireRes,ColdRes,LightningRes,Armour;
    public float Hp,MS,AS, damage;

    public PlayerData(PlayerController player)
    {
        UnlockedSlots = player.UnlockedSlot;
        coins = player.coins;
        Hp = player.Hp;
        MS = player.MovementSpeed;
        AS = player.AttackSpeed;
        FireRes = player.FireResistance;
        ColdRes = player.ColdResistance;
        LightningRes = player.LightningResistance;
        Armour = player.Armour;
        damage = player.Damage;

    }
}
