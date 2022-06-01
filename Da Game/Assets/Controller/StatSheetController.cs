using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatSheetController : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI statsText;
    public GameObject statSheetsWindow;
    void Awake()
    {
        if (statSheetsWindow == null)
            statSheetsWindow = GameObject.Find("Canvas").transform.Find("Stat Sheet Window").gameObject;
        // PlayerController.instance = PlayerController.instance;
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
        if (Input.GetKeyDown(KeyCode.Tab) && statSheetsWindow!=null)
        {
            if (!statSheetsWindow.activeSelf)
            {
                if (PlayerController.instance == null)
                    Debug.Log("pc is null");
                if (statsText == null)
                    Debug.Log("stats text is null");
                statSheetsWindow.SetActive(true);
                UpdatesStatSheet();

            }
        }
        if (Input.GetKeyUp(KeyCode.Tab) && statSheetsWindow!=null )
        {
            if (statSheetsWindow.activeSelf)
            {
                statSheetsWindow.SetActive(false);

            }
        }
    }
    private void UpdatesStatSheet()
    {
        string FireType="Single Fire Mode";
        if (PlayerController.instance.FireType != 0)
            FireType = "Spread Fire Mode";

        statsText.text = "<color=green>" + PlayerController.instance.Hp.ToString("0.##") + "/" + PlayerController.instance.MaxHP + "</color>\n" +
                         PlayerController.instance.Damage.ToString("0.##") + "\n" +
                         PlayerController.instance.FireRate.ToString("0.##") + "\n" +
                         FireType + "\n" +
                         PlayerController.instance.DamageType_.ToString() +"\n"+
                         PlayerController.instance.MovementSpeed + "\n" +
                         "<color=black>" + PlayerController.instance.Armour + "</color>\n" +
                         "<color=red>" + PlayerController.instance.FireResistance + "</color>\n" +
                         "<color=lightblue>" + PlayerController.instance.ColdResistance + "</color>\n" +
                         "<color=yellow>" + PlayerController.instance.LightningResistance + "</color>";
    }
    public void ReloadStatSheetWindow()
    {
        if (statSheetsWindow == null)
            statSheetsWindow = GameObject.Find("Canvas").transform.Find("Stat Sheet Window").gameObject;
        //pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (statSheetsWindow!=null)
            statsText = statSheetsWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        else Debug.Log("Stat sheet window is null can't open stat sheet");
    }
}
