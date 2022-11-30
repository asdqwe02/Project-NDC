using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ArcherController : Archer
{
    [SerializeField] CircleCollider2D collider2D;
    private Vector2 direction;

    public float nextWaypointDistance = 3f;
    //GFX
    bool FacingRight = true;

    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    private Vector3 StartingPosition;
    private Vector3 RoamPosition;

    Seeker seeker;

    private void Awake()
    {
        base.Awake();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        target = PlayerController.instance.transform;
        StartingPosition = transform.position;
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

        // CheckLife();

        FindTarget();

        if (!statusEffects.Contains(StatusEffect.Freeze))
            ProcessAction();

    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    public override void OnDeath(object sender, OnDeathEventArgs e)
    {
        base.OnDeath(sender, e);
        // collider2D.enabled = false;
        // Drop();
        // animator.SetBool("IsDying", true);
    }


    // void CheckLife() // maybe moving this to Enenmy class
    // {
    //     if (Hp <= 0)
    //     {
    //         isDying = true;
    //         collider2D.enabled = false;
    //         Drop();
    //         animator.SetBool("IsDying", true);

    //     }
    // }

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
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
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
        float scalar = 0.3f;
        Vector3 vecTemp = target.position;
        vecTemp.z = transform.position.z;
        Vector3 aimDirection = (vecTemp - _firePoint.position).normalized;
        //Debug.Log("Range Enemy Aim Direction Magnitude" + aimDirection.magnitude);
        Vector2 KnockBack = new Vector2(aimDirection.x * scalar, aimDirection.y * scalar);
        Transform firedProjectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        firedProjectile.GetComponent<Bullet>().setUp(aimDirection, false, Damage, DamageType_, KnockBack);
        AudioManager.Instance.PlaySound(AudioManager.Sound.Woosh, transform.position);
    }
}
