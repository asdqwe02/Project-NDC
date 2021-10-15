using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class GolemController : Enemy
{
    [SerializeField] CircleCollider2D collider2D;
    Rigidbody2D rb;

    float MaxHP;


    bool switchPhase2 = false;
    bool switchPhase3 = false;

    public Transform wayPoint01, wayPoint02, wayPoint03, wayPoint04, wayPoint05;
    public float toWayPoint = 1;
    bool FacingRight = false;
    bool EndOfFold = false;
    bool IsFolded = false;
    bool normalizeLaser = false;
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

    [Header("Cooldown and Timer")]
    public float IdleTimer = 1;
    [SerializeField] private float SlamTimer, SlamCooldown = 2;
    [SerializeField] private float RollnLaserTimer, RollnLaserCooldown = 5;
    public float RoamTimer = 1;

    [Header("Prefabs and Components")]
    [SerializeField] private Transform _slamPrefab;
    [SerializeField] private Transform _laserPoint;
    private Laser AccessChildLaser;

    [Header("Attack Point")]
    [SerializeField] private Transform _slamPoint;

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
        AccessChildLaser = gameObject.GetComponentInChildren<Laser>();
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
        Debug.Log(state);
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
        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();
        if (state == State.Run)
        {
            animator.SetBool("IsRunning", true);
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
        }

        if(state == State.Fold)
        {
            if (IsFolded)
                Velocity = new Vector2(direction.x * movementSpeed * 3, direction.y * movementSpeed * 3);
            else
                Velocity = new Vector2(0, 0);
            animator.SetBool("IsFolding", true);
            animator.SetBool("IsRunning", false);
            normalizeLaser = false; // 
        }
        if(state == State.Laser)
        {
            AccessChildLaser.start = true;
            if (!normalizeLaser)
            {
                AccessChildLaser.transform.eulerAngles = new Vector3(0, 0, 0); // called only once in this state
                normalizeLaser = true;
            }
            animator.SetBool("IsFolding", false);
            animator.SetBool("IsLasering", true);

        }
        if(state == State.Roam)
        {
            animator.SetBool("IsLasering", false);
            animator.SetBool("IsRunning", true);

        }
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        rb.velocity = Velocity;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if ((animator.GetBool("IsSlamming") && collision.gameObject.tag == "Player") || (animator.GetBool("IsFolding") && collision.gameObject.tag == "Player"))
        //{
        //    float scalar = 0.3f;
        //    Vector2 KnockBack = new Vector2(direction.x * scalar, direction.y * scalar);
        //    collision.gameObject.GetComponent<PlayerController>().takeDamage(Damage, KnockBack);
        //}
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
            if (Vector3.Distance(transform.position, target.transform.position) <= AttackRange && SlamTimer==SlamCooldown)
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
                    IdleTimer = 1; // reset the timer
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
                {
                    state = State.Laser;
                    EndOfFold = false;
                }
                    

            }
            if(state == State.Laser)
            {
                if (AccessChildLaser.endOfLaser)
                {
                    state = State.Roam;
                    AccessChildLaser.endOfLaser = false ;
                    AccessChildLaser.start = false;
                    InvokeRepeating("RollandLaserCDTimer", 0f, 0.5f);
                }
            }
            if (state == State.Roam)
            {

                RoamTimer -= Time.deltaTime;
                if (RoamTimer <= 0)
                {
                    state = State.Idle;
                    RoamTimer = 1;
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
    public void Slam()
    {
        animator.SetBool("IsSlamming", false);
        float scalar = 0.3f;
        Vector2 KnockBack = new Vector2(direction.x * scalar, direction.y * scalar);
        Transform GolemSlam = Instantiate(_slamPrefab, _slamPoint.position, Quaternion.identity);
        GolemSlam.GetComponent<Slam>().SetUp(Damage, KnockBack);
        if ((target.position.x - transform.position.x > 0 && FacingRight == true) || (target.position.x - transform.position.x < 0 && FacingRight == false))
        {
            flip();
            GolemSlam.Rotate(0f, 0f, 180f);
        }
        InvokeRepeating("SlamCDTimer", 0f, 0.5f);
    }

    public void HasFolded()
    {
        
        IsFolded = true;
    }
    private void SlamCDTimer()
    {
        SlamTimer -= 0.5f;
        if (SlamTimer <= 0)
        {
            SlamTimer = SlamCooldown;
            CancelInvoke("SlamCDTimer");
        }

    }
    private void RollandLaserCDTimer()
    {
        RollnLaserTimer -= 0.5f;
        if (RollnLaserTimer <= 0)
        {
            RollnLaserTimer = RollnLaserCooldown;
            CancelInvoke("RollandLaserCDTimer");
        }

    }
}
