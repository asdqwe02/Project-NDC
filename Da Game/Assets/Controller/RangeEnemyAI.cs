using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class RangeEnemyAI : Enemy
{
    [SerializeField] CircleCollider2D collider2D;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _projectilePrefab;
    private bool isDying = false;//Bloat TODO:delete


    private enum State
    {
        Roaming,
        Aiming,
    }

    private Vector2 direction;
    private double targetRange = 10;
    [SerializeField] private float nextAttackTime = 0f;
    private State state;
    public Transform target;
    public float nextWaypointDistance = 3f;
    //GFX
    bool FacingRight = true;
    public Animator animator;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    private Vector3 StartingPosition;
    private Vector3 RoamPosition;

    Seeker seeker;
    Rigidbody2D rb;



    private void Start()
    {
        target = PlayerController.instance.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        StartingPosition = transform.position;
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, .5f);
        nextAttackTime -= Time.deltaTime; //Next attack time start value

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            if (state == State.Aiming)
                seeker.StartPath(rb.position, rb.position, OnPathComplete);
            else if (state == State.Roaming)
                seeker.StartPath(rb.position, GetRoamingPosition(), OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        //next fire time calculation
        if (nextAttackTime > 0)
        {
            nextAttackTime = nextAttackTime - Time.deltaTime;
        }
        else if (!statusEffects.Contains(StatusEffect.Freeze))
        {
            animator.SetBool("IsAttacking", true);
            if (state == State.Aiming)
                FireProjectile();

            nextAttackTime += 1 / AttackSpeed;
        }
    }
    private void FixedUpdate()
    {

        CheckLife();

        FindTarget();

        if (!statusEffects.Contains(StatusEffect.Freeze))
            ProcessAction();

    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }



    void CheckLife() // maybe moving this to Enenmy class
    {
        if (Hp <= 0)
        {
            isDying = true;
            collider2D.enabled = false;
            DropItem();
            animator.SetBool("IsDying", true);

        }
    }

    private Vector3 GetRoamingPosition()
    {
        return StartingPosition + Utilities.GetRandomDir() * Random.Range(10f, 7f);
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < targetRange)
        {
            //player within target range
            state = State.Aiming;
        }
        else
            state = State.Roaming;
    }
    private void ProcessAction()
    {
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
        }


        if (path == null)
        {
            return;
        }
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        //move with a velocity
        Vector2 Velocity = new Vector2(direction.x * MovementSpeed, direction.y * MovementSpeed);

        if (animator.GetBool("IsDying") == true || State.Aiming == state)
        {
            Velocity = new Vector2(0, 0);
            animator.SetBool("IsAttacking", true);
        }
        else
        {
            animator.SetBool("IsAttacking", false);

        }
        rb.velocity = Velocity;

        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();
        if (rb.velocity == new Vector2(0, 0))
        {
            if ((target.position.x > transform.position.x && FacingRight == false) || (target.position.x < transform.position.x && FacingRight == true))
            {
                flip();
            }
        }
        if (direction.magnitude == 0)
        {
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    private void FireProjectile()
    {
        //Vector2 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);



        //Do this bc we don't have separate gun from the body yet
        float scalar = 0.3f;
        Vector3 aimDirection = (target.position - _firePoint.position).normalized;
        Vector2 KnockBack = new Vector2(aimDirection.x * scalar, aimDirection.y * scalar);
        Transform firedProjectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);

        //Replace DamageType enum with a variable damageType later
        firedProjectile.GetComponent<Bullet>().setUp(aimDirection, false, Damage, DamageType_, KnockBack);
    }
}
