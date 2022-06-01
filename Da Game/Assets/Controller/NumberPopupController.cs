using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPopupController : MonoBehaviour
{
    //bool isDamagePopup = false; //bad idea 

    // Start is called before the first frame update
    void Start()
    {
        //transform.localPosition += new Vector3(0, 0.5f, 0);
        float randomXPos = Random.Range(-0.25f, 0.25f);
        float randomYPos = Random.Range(0.25f, 0.5f);
        transform.localPosition += new Vector3(randomXPos, randomYPos, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HealingNumberSetUp(float healHp)
    {
        transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(108,187,60,255);
        transform.GetChild(0).GetComponent<TextMeshPro>().text = healHp.ToString();
    }
    public void DamageNumberSetUp(float damage, MovingObjects.DamageType damageType)
    {
        TextMeshPro number = new TextMeshPro(); //don't have a use for this yet
        switch (damageType)
        {
            case MovingObjects.DamageType.Physical:
                transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(147, 89, 16, 255);
                break;
            case MovingObjects.DamageType.Fire:
                transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(255, 14, 0, 255);
                break;
            case MovingObjects.DamageType.Cold:
                transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(0, 255, 227, 255);
                break;
            case MovingObjects.DamageType.Lightning:
                //transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color(255, 14, 0);
                transform.GetChild(0).GetComponent<TextMeshPro>().color = new Color32(245, 255, 0, 255);
                break;
            default:
                break;
        }
        transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
        //transform.GetChild(0).GetComponent<TextMesh>().color = number.color;

    }

}
