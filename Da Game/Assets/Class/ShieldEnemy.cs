using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ShieldEnemy : Enemy
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

    [SerializeField]
    public double shieldHP = 0;
    protected double shieldBaseHP = 0;
    protected float shieldPercent = 0;
    protected bool shieldActive = false;
    private GameObject shield;

    public GameObject Shield { get => shield; set => shield = value; }
}
