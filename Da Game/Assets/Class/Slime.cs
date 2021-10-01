using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MovingObjects
{
    public Animator animator;

    //this is a really bad fix to disable collison need TODO:find a better solution
    [SerializeField] CircleCollider2D collider2D;

    private bool isDying = false;//Bloat TODO:delete
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
        {
            isDying = true;
            collider2D.enabled = false;
            animator.SetBool("IsDying", isDying);
        }
            
    }
}
