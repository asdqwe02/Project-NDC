using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Archer : Enemy
{
    protected bool isDying = false;//Bloat TODO:delete

    protected enum State
    {
        Roaming,
        Aiming,
    }
    protected State state;
    public Transform target;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Transform _projectilePrefab;

    protected double targetRange = 10;
    [SerializeField] protected float nextAttackTime = 0f;
    protected Rigidbody2D rb;
    public Animator animator;
    protected Path path;
  
}
