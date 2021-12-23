using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CircularHPBar : MonoBehaviour
{
    public Image CircleBar;
    public Image ExtraBar;
    public TextMeshProUGUI HealthDisplay;

    public float CurrentHP;
    public float MaxHP;

    public float circlePercentage = 0.3f;
    private float circleFillAmount = 1f;


    // Update is called once per frame
    void Update()
    {
        CircleFill();
        ExtraFill();
    }
    private void FixedUpdate()
    {
        UpdateHP();

    }


    private void UpdateHP()
    {
        CurrentHP = PlayerController.instance.hp;
        MaxHP = PlayerController.instance.MaxHP;

        string HP_Text = CurrentHP.ToString("R") + " / " + MaxHP.ToString("R");
        HealthDisplay.text = HP_Text;
    }
    private void CircleFill()
    {
        float HealthPercentage = CurrentHP / MaxHP;
        float circleFill = HealthPercentage / circlePercentage;

        circleFill *= circleFillAmount;
        CircleBar.fillAmount = circleFill;
    }
    private void ExtraFill()
    {
        float circleAmount = circlePercentage * MaxHP;
        float extraHealth = CurrentHP - circleAmount;
        float extraToHealth = MaxHP - circleAmount;
        float extraFill = extraHealth / extraToHealth;
        ExtraBar.fillAmount = extraFill;
    }
}
