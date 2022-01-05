using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatSheetController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController pc;
    TextMeshProUGUI statsText;
    public GameObject statSheetsWindow;
    void Start()
    {
        pc = PlayerController.instance;
        //pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (statSheetsWindow!=null)
            statsText = statSheetsWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        else Debug.Log("Stat sheet window is null can't open stat sheet");
    }
    private void Update()
    {
        CheckInput();
    }
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!statSheetsWindow.active)
            {
                if (pc == null)
                    Debug.Log("pc is null");
                if (statsText == null)
                    Debug.Log("stats text is null");
                statSheetsWindow.SetActive(true);
                UpdatesStatSheet();

            }
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (statSheetsWindow.active)
            {
                statSheetsWindow.SetActive(false);

            }
        }
    }
    private void UpdatesStatSheet()
    {
        string FireType="Single Fire Mode";
        if (pc.FireType != 0)
            FireType = "Spread Fire Mode";

        statsText.text = "<color=green>" + pc.Hp.ToString("0.##") + "/" + pc.MaxHP + "</color>\n" +
                         pc.Damage.ToString("0.##") + "\n" +
                         pc.FireRate.ToString("0.##") + "\n" +
                         FireType + "\n" +
                         pc.DamageType_.ToString() +"\n"+
                         pc.MovementSpeed + "\n" +
                         "<color=black>" + pc.Armour + "</color>\n" +
                         "<color=red>" + pc.FireResistance + "</color>\n" +
                         "<color=lightblue>" + pc.ColdResistance + "</color>\n" +
                         "<color=yellow>" + pc.LightningResistance + "</color>";
    }
}
