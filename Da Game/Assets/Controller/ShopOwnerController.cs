using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopOwnerController : MonoBehaviour
{
    public GameObject Shop;
    private TextMeshProUGUI textToDisplay;
    public int MaxSlot = 5;
    public void displayBuyOption()
    {
        textToDisplay = Shop.GetComponent<TextMeshProUGUI>();
        int unlockedSlots = PlayerController.Singleton.UnlockedSlot;
        if (unlockedSlots < MaxSlot)
        {
            string text = "Unlockable Slot " + unlockedSlots + 1;
            text = text +" : "+ CalculatePrice(unlockedSlots) + " gold";
            textToDisplay.text = text.ToString();
        }
        else
        {
            textToDisplay.text = "Reached maximum unlockable slots";
        }
    }

    public void tryBuy()
    {
        int currentMoney = PlayerController.Singleton.coins;
        int UnlockedSlot = PlayerController.Singleton.UnlockedSlot;
        if(currentMoney >= CalculatePrice(UnlockedSlot))
        {
            PlayerController.Singleton.coins = (int)(currentMoney - CalculatePrice(UnlockedSlot));
            displayBuyOption();
        }
    }
    public float CalculatePrice(float unlockedSlot)
    {
        return Mathf.Pow(2, unlockedSlot) * 1000;
    }

}
