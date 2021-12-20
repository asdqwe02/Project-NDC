using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : NonMovingObject
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }
    private void FixedUpdate()
    {
        
    }

    //This might look weird because crate doesn't have IsTrigger flag active buttt this is how it work
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("bullet"))
        {

            takdeDamage(1);
            if (checkDestroy())
            {
                animator.SetTrigger("Destroyed");
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
