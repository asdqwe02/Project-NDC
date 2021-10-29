using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int UnlockedSlots;
    public int Money;
    
    public PlayerData(PlayerController player)
    {
        UnlockedSlots = player.UnlockedSlot;
        Money = player.Money;
    }
}
