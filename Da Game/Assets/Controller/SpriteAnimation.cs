using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] frameArray;
    private int currentFrame;
    private float timer;
    private float FrameRate = .1f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Udpate()
    {
        timer += Time.deltaTime;
        if(timer >= FrameRate)
        {
            timer -= FrameRate;
            currentFrame = (currentFrame + 1) % frameArray.Length;
            spriteRenderer.sprite = frameArray[currentFrame];
            Destroy(gameObject, 0.5f);
        }
    }
}
