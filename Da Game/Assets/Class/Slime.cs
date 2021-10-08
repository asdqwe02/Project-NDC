using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Slime : MovingObjects
{
    

    //this is a really bad fix to disable collison need TODO:find a better solution

    [SerializeField] CircleCollider2D collider2D;
    private bool isDying = false;//Bloat TODO:delete
                                 // Start is called before the first frame update

    private enum State
    {
        Roaming,
        ChaseTarget,
    }

    private Vector2 direction;
    private double AttackRange = 1.8;
    private float nextAttackTime;
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
        target = PlayerController.Singleton.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        StartingPosition = transform.position;

        InvokeRepeating("UpdatePath", 0f, .5f);

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            if(state == State.ChaseTarget)
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
            else if(state == State.Roaming)
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

    private void FixedUpdate()
    {
        CheckLife();

        FindTarget();


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

        Vector2 Velocity = new Vector2(direction.x * movementSpeed, direction.y * movementSpeed);

        if (animator.GetBool("IsDying") == true || Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
        {
            Velocity = new Vector2(0, 0);
            if(Time.time > nextAttackTime)
            {
                animator.SetBool("IsAttacking", true);
                nextAttackTime = Time.time + 1/attackSpeed;
            }
            

        }
        else
        {
            animator.SetBool("IsAttacking", false);

        }
        rb.velocity = Velocity;




        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();
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

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }



    void CheckLife()
    {
        if (hp <= 0)
        {
            isDying = true;
            collider2D.enabled = false;
            animator.SetBool("IsDying", true);
            
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return StartingPosition + Ultilities.GetRandomDir() * Random.Range(10f, 7f);
    }

    private void FindTarget()
    {
        float targetRange = 10f;
        if (Vector3.Distance(transform.position, target.transform.position) < targetRange)
        {
            //player within target range
            state = State.ChaseTarget;
        }
        else
            state = State.Roaming;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(animator.GetBool("IsAttacking") && collision.gameObject.tag == "Player")
        {
            float scalar = 1;
            Vector2 KnockBack = new Vector2(direction.x * scalar, direction.y * scalar);
            collision.gameObject.GetComponent<PlayerController>().takeDamage(Damage, KnockBack);
        }
    }


}
