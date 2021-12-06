using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int UnlockedSlots;
    public int coins;
    
    public PlayerData(PlayerController player)
    {
        UnlockedSlots = player.UnlockedSlot;
        coins = player.coins;
    }
}
