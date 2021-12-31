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
        int unlockedSlots = PlayerController.instance.UnlockedSlot;

        if (unlockedSlots < MaxSlot)
        {
            int temp = unlockedSlots + 1;
            string text = "Unlockable Slot " + temp;
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
        int currentMoney = PlayerController.instance.coins;
        int UnlockedSlot = PlayerController.instance.UnlockedSlot;
        if(currentMoney >= CalculatePrice(UnlockedSlot))
        {
            PlayerController.instance.coins = (int)(currentMoney - CalculatePrice(UnlockedSlot));
            PlayerController.instance.UnlockedSlot += 1;
            PlayerController.instance.Save();
            displayBuyOption();
        }
    }
    public float CalculatePrice(float unlockedSlot)
    {
        return Mathf.Pow(2, unlockedSlot) * 10000;
    }

}
