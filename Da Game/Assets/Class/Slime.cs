using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MovingObjects
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (hp <= 0)
            Destroy(gameObject);
    }
}
