using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSheetController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController pc;
    void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
