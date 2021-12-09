using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoinCounterController : MonoBehaviour
{
    PlayerController pc;
    int CoinsCounter = 0;
    TextMeshProUGUI CoinNumber;
    private void Start()
    {
        pc = PlayerController.instance;
        //pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CoinNumber = GetComponent<TextMeshProUGUI>();
        UpdateCounter();
    }

    private void FixedUpdate()
    {
        if (CoinsCounter != pc.coins)
            UpdateCounter();
    }
    private void UpdateCounter()
    {
        CoinsCounter = pc.coins;
        CoinNumber.text = CoinsCounter.ToString();
    }
}
