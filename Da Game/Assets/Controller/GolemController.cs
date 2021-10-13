using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class GolemController : Enemy
{
    [SerializeField] CircleCollider2D collider2D;
    Rigidbody2D rb;
    float IdleTimer = 2;
    float SlamTimer = 2;
    float LaserTimer = 2;
    float RollTimer = 2;

    float MaxHP;


    bool switchPhase2 = false;
    bool switchPhase3 = false;

    public Transform wayPoint01, wayPoint02, wayPoint03, wayPoint04, wayPoint05;
    public float toWayPoint = 1;
    bool reachedWayPoint = false;
    bool FacingRight = false;
    bool EndOfFold = false;
    private enum State
    {
        Slam,
        Idle,
        Run,
        Fold,
        Laser,
        Roam
    }

    private enum Phase
    {
        FirstPhase,
        SeccondPhase,
        LastPhase
    }
    private Vector2 direction;
    private Phase phase;
    private State state;
    public Transform target;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    Seeker seeker;
    private Vector3 StartingPosition;
    public float nextWaypointDistance = 3f;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.Singleton.transform;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        StartingPosition = transform.position;
        animator = GetComponent<Animator>();
        phase = Phase.FirstPhase;
        MaxHP = hp;
        InvokeRepeating("UpdatePath", 0f, .5f);

    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if(state == State.Idle || state == State.Laser || state == State.Slam)
            {
                seeker.StartPath(rb.position, rb.position, OnPathComplete);
            }
            if(state == State.Run)
            {
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
            }
            if(state == State.Fold)
            {
                switch (toWayPoint)
                {
                    case 1:
                        seeker.StartPath(rb.position, wayPoint01.position, OnPathComplete);
                        break;
                    case 2:
                        seeker.StartPath(rb.position, wayPoint02.position, OnPathComplete);
                        break;
                    case 3:
                        seeker.StartPath(rb.position, wayPoint03.position, OnPathComplete);
                        break;
                    case 4:
                        seeker.StartPath(rb.position, wayPoint04.position, OnPathComplete);
                        break;
                    case 5:
                        seeker.StartPath(rb.position, wayPoint05.position, OnPathComplete);
                        break;
                }
            }
            if(state == State.Roam)
            {
                seeker.StartPath(rb.position, GetRoamingPosition(), OnPathComplete);
            }
            
        }
           
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

    }
    void FixedUpdate()
    {
        updatePhase();
        updateState();
        CheckLife();
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
        
        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();
        if (state == State.Run)
        {
            Vector2 Velocity = new Vector2(direction.x * movementSpeed, direction.y * movementSpeed);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsSlamming", false);
            rb.velocity = Velocity;
        }
        else if(state == State.Slam)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsSlamming", true);

        }
        if(state == State.Idle)
        {
            rb.velocity = new Vector3(0,0,0);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsSlamming", false);
        }

        if(state == State.Fold)
        {
            Vector2 Velocity = new Vector2(direction.x * movementSpeed * 3, direction.y * movementSpeed * 3);
            animator.SetBool("IsFolding", true);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsSlamming", false);
            rb.velocity = Velocity;
        }
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((animator.GetBool("IsSlamming") && collision.gameObject.tag == "Player") || (animator.GetBool("IsFolding") && collision.gameObject.tag == "Player"))
        {
            float scalar = 0.3f;
            Vector2 KnockBack = new Vector2(direction.x * scalar, direction.y * scalar);
            collision.gameObject.GetComponent<PlayerController>().takeDamage(Damage, KnockBack);
        }
    }


    void updatePhase()
    {

        if ((hp > (MaxHP * 0.3)) && (hp < (MaxHP * 0.6)))
        {
            phase = Phase.SeccondPhase;
            if (switchPhase2 == false)
            {
                state = State.Idle;
                switchPhase2 = true;
            }

            
        }
        else if(hp <= MaxHP * 0.3)
        {
            phase = Phase.LastPhase;
        }
    }
    void updateState()
    {

        if(phase == Phase.FirstPhase)
        {
            double AttackRange = 5;
            if (Vector3.Distance(transform.position, target.transform.position) <= AttackRange)
            {
                state = State.Slam;
            }
            else
            {
                state = State.Run;
            }
        }
        else if (phase == Phase.SeccondPhase)
        {
            if(state == State.Idle)
            {
                
                IdleTimer = IdleTimer - Time.deltaTime;
                
                if (IdleTimer < 0)
                {

                    state = State.Fold; 

                    IdleTimer = 2; // reset the timer
                }
            }
            if (state == State.Fold)
            {
                if (!EndOfFold)
                {
                    if (Vector3.Distance(transform.position, wayPoint01.position) <= 1.3 && toWayPoint == 1)
                    {
                        toWayPoint++;
                    }
                    if (Vector3.Distance(transform.position, wayPoint02.position) <= 1.3 && toWayPoint == 2)
                    {
                        toWayPoint++;
                    }
                    if (Vector3.Distance(transform.position, wayPoint03.position) <= 1.3 && toWayPoint == 3)
                    {
                        toWayPoint++;
                    }
                    if (Vector3.Distance(transform.position, wayPoint04.position) <= 1.3 && toWayPoint == 4)
                    {
                        toWayPoint++;
                    }
                    if (Vector3.Distance(transform.position, wayPoint05.position) <= 1.3 && toWayPoint == 5)
                    {
                        toWayPoint = 1;
                        EndOfFold = true;
                    }
                }
                else
                    state = State.Laser;

            }
            if(state == State.Laser)
            {
                LaserTimer -= Time.deltaTime;
                if (LaserTimer < 0)
                {
                    state = State.Roam;
                    LaserTimer = 2; // reset the timer
                }
            }
            if (state == State.Roam)
            {
                if (reachedWayPoint)
                {
                    state = State.Idle;
                }
            }

        }
        else if (phase == Phase.LastPhase)
        {

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
            collider2D.enabled = false;
            animator.SetBool("IsDying", true);

        }
    }
    private Vector3 GetRoamingPosition()
    {
        return StartingPosition + Ultilities.GetRandomDir() * Random.Range(10f, 7f);
    }
}
