using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MovingObjects
{
    public Animator animator;
    public float timer, baseTimer = 5;
    void Start()
    {
        timer = baseTimer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        LastTakenDamageCountDown();
    }
    public override void takeDamage(float damageTaken, DamageType damageTypeTaken)
    {
        base.takeDamage(damageTaken, damageTypeTaken);
        animator.SetBool("Damaged",true);
        timer = baseTimer;
    }
    private void LastTakenDamageCountDown()
    {
        if (animator.GetBool("Damaged")) { 
            timer -= Time.deltaTime;
            if (timer <= 0) {
                animator.SetBool("Damaged", false);
                timer = baseTimer;
                Hp = 1000000000;
            }
                
        }
    }
    private void CheckHPOverFlow()
    {
        
    }
}
