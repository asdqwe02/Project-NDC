using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Bomber : Enemy
{
   protected enum State
    {
        Roaming,
        ChaseTarget,
    }
    protected double AttackRange = 1.5f;
    protected State state;
    public Transform target;
    //GFX
    public Animator animator;

    protected Path path;
  
    protected Rigidbody2D rb;

    public Transform explodePrefab;

}
