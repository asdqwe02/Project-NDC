using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObjects
{
    public Transform coinsPrefab;
    [Header("Coin drop range")]
    public int minCoinDrop = 0;
    public int maxCoinDrop = 0;
    private bool dropped=false;

    public void DropCoins()
    {
        if (!dropped)
        {
            int randomAmountofCoin = Random.RandomRange(minCoinDrop, maxCoinDrop);
            coinsPrefab.GetComponent<CoinController>().setUpCoinDrop(randomAmountofCoin);
            Instantiate(coinsPrefab, transform.position, Quaternion.identity);
            dropped = true;
        }
        
    }
}
