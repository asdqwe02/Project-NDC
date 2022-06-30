using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObjects
{
    [Header("Drop Prefabs")]
    public Transform coinsPrefab;
    public Transform healthPotionPrefab;
    public Transform itemPrefab;
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
            int randomAmountofCoin = Random.Range(minCoinDrop, maxCoinDrop+1);
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
        if (!dropped)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
            dropped = true;
        }
    }
    public void Drop()
    {
        // itemRNG = new RNG();
        // if (itemRNG.RollNumber(20f))
        //     DropPotion();
        // else DropCoins();

        float chance = Random.Range(0f,1f);
        // Debug.Log(chance);
        switch (chance)
        {
            case var x when (x <=0.2f):
                DropItem();
                break;
            case var x when (x >0.2f && x <=0.45f):
                DropPotion();
                break;
            case var x when (x > 0.45f):
                DropCoins();
                break;
            default:
                break;
        }            
    }
    public override void takeDamage(float damageTaken, DamageType damageTypeTaken, Vector2 KnockBack)
    {
        base.takeDamage(damageTaken, damageTypeTaken, KnockBack);
        GetComponent<Rigidbody2D>().velocity = KnockBack * knockBackForce;
        //GetComponent<Rigidbody2D>().AddForce(KnockBack*knockBackForce);
    }
}
