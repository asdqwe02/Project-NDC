using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine.SceneManagement;

public class FileSlotDisplayController : MonoBehaviour
{
    public GameObject UnlockedSlotToDisplay1,MoneyToDisplay1, UnlockedSlotToDisplay2, MoneyToDisplay2, UnlockedSlotToDisplay3, MoneyToDisplay3, FoundFileDialog1,NotFoundFileDialog1, FoundFileDialog2, NotFoundFileDialog2, FoundFileDialog3, NotFoundFileDialog3;
    public TextMeshProUGUI bufferUnlockedSlot,bufferMoney;
    public void DisplaySlotFile()
    {
        //slot 1
        PlayerData data = SaveSytemManagement.LoadPlayer("1");
        bufferUnlockedSlot = UnlockedSlotToDisplay1.GetComponent<TextMeshProUGUI>();
        bufferMoney = MoneyToDisplay1.GetComponent<TextMeshProUGUI>();
        if (data != null)
        {
            bufferUnlockedSlot.text = data.UnlockedSlots.ToString();
            bufferMoney.text = data.coins.ToString();
        }
        else
        {
            bufferUnlockedSlot.SetText("None");
            bufferMoney.SetText("None");

        }
        //slot 2
        data = SaveSytemManagement.LoadPlayer("2");
        bufferUnlockedSlot = UnlockedSlotToDisplay2.GetComponent<TextMeshProUGUI>();
        bufferMoney = MoneyToDisplay2.GetComponent<TextMeshProUGUI>();
        if (data != null)
        {
            bufferUnlockedSlot.text = data.UnlockedSlots.ToString();
            bufferMoney.text = data.coins.ToString();
        }
        else
        {
            bufferUnlockedSlot.SetText("None");
            bufferMoney.SetText("None");

        }
        //slot 3
        data = SaveSytemManagement.LoadPlayer("3");
        bufferUnlockedSlot = UnlockedSlotToDisplay3.GetComponent<TextMeshProUGUI>();
        bufferMoney = MoneyToDisplay3.GetComponent<TextMeshProUGUI>();
        if (data != null)
        {
            bufferUnlockedSlot.text = data.UnlockedSlots.ToString();
            bufferMoney.text = data.coins.ToString();
        }
        else
        {
            bufferUnlockedSlot.SetText("None");
            bufferMoney.SetText("None");

        }
    }

    public void LoadDialog(string slot)
    {
        PlayerData data = SaveSytemManagement.LoadPlayer(slot);
        if (data != null) 
        {
            switch(slot)
            {
                case "1":
                    FoundFileDialog1.SetActive(true);
                    break;
                case "2":
                    FoundFileDialog2.SetActive(true);
                    break;
                case "3":
                    FoundFileDialog3.SetActive(true);
                    break;
            }
            
        }
        else
        {
            switch (slot)
            {
                case "1":
                    NotFoundFileDialog1.SetActive(true);
                    break;
                case "2":
                    NotFoundFileDialog2.SetActive(true);
                    break;
                case "3":
                    NotFoundFileDialog3.SetActive(true);
                    break;
            }
        }
            
        
    }
    public void Load(string slot)
    {
        PlayerController.slot = slot;
        PlayerController.IsLoading = true;
        PlayerController.To_Load = true;
        ResetScene();
        SceneManager.LoadScene("Hideout");
    }
    public void Create(string slot)
    {
        PlayerController.slot = slot;
        PlayerController.IsLoading = false;
        PlayerController.To_Load = true;
        ResetScene();
        SceneManager.LoadScene("Hideout");
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
