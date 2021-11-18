using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginWaves : MonoBehaviour
{
    public bool start = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            start = true;
    }
}
