using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarController : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 offset;
    public float value, maxvalue;
    
    public void setHealth(float Health,float maxHealth)
    {
        slider.gameObject.SetActive(Health < maxHealth);
        slider.value = Health;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }
    // Update is called once per frame
    void Update()
    {
        
        value = slider.value;
        maxvalue = slider.maxValue;
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
