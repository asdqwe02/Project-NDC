using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarController : MonoBehaviour
{
    public Vector3 offset;
    public Slider Slider;
    public Color low;
    public Color high;

    public void setHealth(float Health, float maxHealth)
    {
        //Slider.gameObject.SetActive(Health < maxHealth);
        Slider.value = Health;
        Slider.maxValue = maxHealth;
        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, Slider.normalizedValue);



    }
    private void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(PlayerController.instance.transform.position + offset);
        setHealth(PlayerController.instance.GetHealth(), PlayerController.instance.MaxHP);
    }

}
