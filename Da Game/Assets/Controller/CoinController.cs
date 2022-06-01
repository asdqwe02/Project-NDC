using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : Interactable
{
    // Start is called before the first frame update
    public Sprite[] coinSprites;
    public int coinAmount = 0;
    private SpriteRenderer spriteRenderer;
    PlayerController pc;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (coinAmount <= 10)
            spriteRenderer.sprite = coinSprites[0];
        else if (10 < coinAmount && coinAmount <= 50)
            spriteRenderer.sprite = coinSprites[1];
        else if (50 < coinAmount && coinAmount <= 100)
            spriteRenderer.sprite = coinSprites[2];
        else if (100 < coinAmount && coinAmount <= 200)
            spriteRenderer.sprite = coinSprites[3];
        else if (200 < coinAmount && coinAmount <= 400)
            spriteRenderer.sprite = coinSprites[4];
          else if (500<=coinAmount)
            spriteRenderer.sprite = coinSprites[4];

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // pc = collision.gameObject.GetComponent<PlayerController>();
            Interact();
        }
    }
    public void setUpCoinDrop(int CoinAmount)
    {
        coinAmount = CoinAmount;
    }
    public override void Interact()
    {
        PlayerController.instance.coins += coinAmount;
        PlayerController.instance.Coin_tobeAdded += coinAmount;
        Destroy(gameObject);
    }
}
