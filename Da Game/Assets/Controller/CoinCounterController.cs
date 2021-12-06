using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinCounterController : MonoBehaviour
{
    PlayerController pc;
    int CoinsCounter = 0;
    Text CoinNumber;
    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CoinNumber = GetComponent<Text>();
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
