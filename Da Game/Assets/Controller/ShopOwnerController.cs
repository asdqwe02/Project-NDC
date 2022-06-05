using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopOwnerController:MonoBehaviour
{
    public GameObject Shop;
    private TextMeshProUGUI textToDisplay;
    public Transform ShopContainer;
    public int MaxSlot = 5;
    public void displayBuyOption()
    {
        int unlockedSlots = PlayerController.instance.UnlockedSlot;
        // textToDisplay = Shop.GetComponent<TextMeshProUGUI>();
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

    public void LoadShopUI()
    {
        ShopContainer =  GameObject.Find("HO_Canvas").transform.Find("Shop Container");
        ShopContainer.Find("Dialouge").Find("Continue Button").GetComponent<Button>().onClick.AddListener(()=>{
            displayBuyOption();
        });
        ShopContainer.Find("Dialouge 2").Find("Buy Option").GetComponent<Button>().onClick.AddListener(()=>{
            tryBuy();
        });
        Shop = ShopContainer.Find("Dialouge 2").Find("Buy Option").Find("Buy Option DIsplay").gameObject; // really shit
        textToDisplay = Shop.GetComponent<TextMeshProUGUI>();
    }
}
