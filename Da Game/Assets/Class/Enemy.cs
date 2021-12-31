using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObjects
{
    [Header("Drop Prefabs")]
    public Transform coinsPrefab;
    public Transform healthPotionPrefab;
    [Header("Coin drop range")]
    public int minCoinDrop = 0;
    public int maxCoinDrop = 0;
    private bool dropped = false;
    private RNG itemRNG;
    protected float knockBackForce = 10f;


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
    public void DropPotion()
    {
        if (!dropped)
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
            dropped = true;
        }

    }
    public void DropItem()
    {
        itemRNG = new RNG();
        if (itemRNG.RollNumber(20f))
            DropPotion();
        else DropCoins();
    }
    public override void takeDamage(float damageTaken, DamageType damageTypeTaken, Vector2 KnockBack)
    {
        base.takeDamage(damageTaken, damageTypeTaken, KnockBack);
        GetComponent<Rigidbody2D>().velocity = KnockBack * knockBackForce;
        //GetComponent<Rigidbody2D>().AddForce(KnockBack*knockBackForce);
    }
}
