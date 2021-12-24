using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PortalHideoutController : Interactable
{
    public GameObject ToDisplay,Menu;
    public TextMeshProUGUI TextToDisplay;
    public string sceneName;
    Collider2D colliderP;
    [SerializeField] private string newScenePassword;
    public override void Interact()
    {
        PlayerController.instance.CloseInteractableIcon();
        PlayerController.instance.Coin_tobeAdded = 0;
        PlayerController.instance.scenePassword = newScenePassword;
        PlayerController.instance.Save();
        PlayerController.instance.InHO = false;
        Activate_Buffs();
        //SceneManager.LoadScene(sceneName);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        TurnOnIIcon(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliderP = null;
        TurnOffIIcon(collision);
    }

    private void AddHP()
    {
        PlayerController.instance.Hp += (int)(PlayerController.instance.Hp * 0.1);
        PlayerController.instance.MaxHP = PlayerController.instance.Hp;
    }

    private void AddMS()
    {
        PlayerController.instance.MovementSpeed += (float)(PlayerController.instance.MovementSpeed * 0.1);
    }

    private void AddAS()
    {
        PlayerController.instance.AttackSpeed += (float)(PlayerController.instance.AttackSpeed * 0.1);
    }

    private void AddArmour()
    {
        PlayerController.instance.Armour += (int)(PlayerController.instance.Armour * 0.1);
    }


    private void Activate_Buffs()
    {
        ToDisplay.SetActive(true); //activate UI
        //Display reveiced buffs 
        string Text_Display = "Buffs received as follow : \n";
        for (int i =0; i< PlayerController.instance.UnlockedSlot;i++)
        {
            System.Random random = new System.Random();
            int x = random.Next(1, 101);
            if (x < 11) // Numbers 1..10 ( A -> 10% )
            {
                AddAS();
                //Display reveiced buffs 
                Text_Display += "+10% Attack Speed \n";
            }
            else if (x < 21) // Numbers 11..20 ( B -> 10 % )
            {
                AddMS();
                //Display reveiced buffs 
                Text_Display += "+10% Movement Speed \n";
            }
            else if (x < 41) // Numbers 21..40 ( C -> 20 % )
            {
                AddArmour();
                //Display reveiced buffs 
                Text_Display += "+10% Armour \n";
            }
            else if (x < 101) // Numbers 41..100 ( D -> 60 % ) 
            {
                AddHP();
                //Display reveiced buffs 
                Text_Display += "+10% HP  \n";
            }
        }
        TextToDisplay.text = Text_Display;

        // Stop time and disable input 
        Time.timeScale = 0f;
        Menu.SetActive(false);


    }

    public void To_Dungeon()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
        Menu.SetActive(true);
    }
}
