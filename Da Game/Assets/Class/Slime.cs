using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // remove this later
public class Slime : Enemy
{
    

    //this is a really bad fix to disable collison need TODO:find a better solution
    //[SerializeField] CircleCollider2D collider2D;//
    private bool isDying = false;//Bloat TODO:delete

    protected enum State
    {
        Roaming,
        ChaseTarget,
    }

    //private Vector2 direction; //
    //bool FacingRight = true;//

    protected double AttackRange = 1.5f;
    protected float nextAttackTime;
    protected State state;
    public Transform target;
    //GFX
    public Animator animator;

    protected Path path;
  
    protected Rigidbody2D rb;
}
