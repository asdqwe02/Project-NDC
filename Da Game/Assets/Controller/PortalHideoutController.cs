using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PortalHideoutController : Interactable
{
    public GameObject ToDisplay, Menu;
    public TextMeshProUGUI TextToDisplay;
    public string sceneName;
    Collider2D colliderP;
    [SerializeField] private string newScenePassword;
    [SerializeField] private Transform statStickPrefab;
    private PlayerClass statStick;
    private Buff.BuffRNG[] buffs;
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

    //Obsolete
    private void AddHP()
    {
        PlayerController.instance.Hp += (int)(PlayerController.instance.Hp * 0.1);
        PlayerController.instance.MaxHP = PlayerController.instance.Hp;
    }
    //Obsolete
    private void AddMS()
    {
        PlayerController.instance.MovementSpeed += (float)(PlayerController.instance.MovementSpeed * 0.1);
    }
    //Obsolete
    private void AddAS()
    {
        PlayerController.instance.AttackSpeed += (float)(PlayerController.instance.AttackSpeed * 0.1);
    }
    //Obsolete
    private void AddArmour()
    {
        PlayerController.instance.Armour += (int)(PlayerController.instance.Armour * 0.1);
    }


    private void Activate_Buffs()
    {
        statStick = statStickPrefab.GetComponent<PlayerClass>();
        buffs = new Buff.BuffRNG[4] {
            new Buff.BuffRNG(Buff.BuffType.PhysicalAttack,100),
            new Buff.BuffRNG(Buff.BuffType.FireAttack,200),
            new Buff.BuffRNG(Buff.BuffType.Armour,300),
            new Buff.BuffRNG(Buff.BuffType.ColdAttack, 200)
        };
        Buff.SortBuffRNGByWeight(ref buffs); //sort buffs
        
        //debug
        string buffrngList = "";
        for (int i = 0; i < buffs.Length; i++)
        {
            buffrngList += buffs[i].buffType.ToString() + ":" + buffs[i].weight.ToString() + " ";
        }
        Debug.Log("buff list: " + buffrngList);

        bool DamageTypeBuffAppeared = false; //use to remove damage type buff from pool
        ToDisplay.SetActive(true); //activate UI
        //Display reveiced buffs 
        string Text_Display = "Buffs received as follow : \n";
        for (int i = 0; i < PlayerController.instance.UnlockedSlot; i++)
        {
            Buff.BuffType BuffToApply = Buff.RollBuffRNG(buffs);
            if (!DamageTypeBuffAppeared) 
                switch (BuffToApply)
                {
                    case Buff.BuffType.PhysicalAttack:
                    case Buff.BuffType.FireAttack:
                    case Buff.BuffType.ColdAttack:
                    case Buff.BuffType.LightningAttack:
                        DamageTypeBuffAppeared = true;
                        Buff.RemoveDamageTypeBuffsFromPool(ref buffs);

                        //Debug
                        /*buffrngList = "";
                        for (int k = 0; k < buffs.Length; k++)
                        {
                            buffrngList += buffs[k].buffType.ToString() + ":" + buffs[k].weight.ToString() + " ";
                        }
                        Debug.Log("buff list after damage type removal: " + buffrngList);*/
                        break;
                    default:
                        break;
                }
            Buff.ApplyBuff(BuffToApply, statStick);
            Text_Display += "+" + BuffToApply.ToString() + "\n";

            //Obsolete delete later
            /* Debug.Log("Buff applied: " + BuffToApply.ToString());

             System.Random random = new System.Random();
             int x = Random.Range(1, 101);
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
             } */

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
