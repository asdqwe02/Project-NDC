using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class GolemController : Enemy
{
    [SerializeField] CircleCollider2D collider2D;
    Rigidbody2D rb;

    float MaxHP;
    float _baseMoveSpeed;

    bool switchPhase2 = false;
    bool switchPhase3 = false;

    public Transform wayPoint01, wayPoint02, wayPoint03, wayPoint04, wayPoint05;
    public List<Transform> WayPoints;
    public float toWayPoint = 1;
    bool FacingRight = false;
    bool EndOfFold = false;
    bool IsFolded = false;
    bool normalizeLaser = false;
    bool pulling = false;
    bool IsSummoning = true;
    bool IsLasering = false;
    public bool DoneSummoningPillars = false;
    private enum State
    {
        Slam,
        Idle,
        Run,
        Fold,
        Laser,
        FiringArm,
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
    public float foldingTimer = 1.5f;
    [SerializeField] private float SlamTimer, SlamCooldown = 2;
    [SerializeField] private float RollnLaserTimer, RollnLaserCooldown = 10;
    [SerializeField] private float PullTimer, PullCooldown = 2;
    public float RoamTimer = 1;

    [Header("Prefabs and Components")]
    [SerializeField] private Transform _slamPrefab;
    [SerializeField] private Transform _armProjectilePrefab;
    [SerializeField] private Transform _pillarsPrefab;
    public GameObject EndStagePopUp;
    private LaserUpdated AccessChildLaser;

    [Header("Attack Point")]
    [SerializeField] private Transform _slamPoint;
    [SerializeField] private Transform _armLaunchPoint;
    [SerializeField] private Transform _pullPoint;

    [Header("Animator Parameter")]
    private bool HasDied = false;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.instance.transform;
        if (EndStagePopUp == null)
            EndStagePopUp = GameObject.Find("EndInstacePortal").gameObject;
        EndStagePopUp.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        StartingPosition = transform.position;
        animator = GetComponent<Animator>();
        phase = Phase.FirstPhase;
        MaxHP = Hp;
        AccessChildLaser = gameObject.GetComponentInChildren<LaserUpdated>();
        WayPointToArray();
        SlamTimer = SlamCooldown;
        RollnLaserTimer = RollnLaserCooldown;
        PullTimer = PullCooldown;
        _baseMoveSpeed = MovementSpeed;
        AudioManager.instance.PlaySoundTrack(AudioManager.SoundTrack.BossST1);
        InvokeRepeating("UpdatePath", 0f, .5f);


    }
    private void WayPointToArray()
    {
        WayPoints.Add(wayPoint01);
        WayPoints.Add(wayPoint02);
        WayPoints.Add(wayPoint03);
        WayPoints.Add(wayPoint04);
        WayPoints.Add(wayPoint05);
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (state == State.Idle || state == State.Laser || state == State.Slam || state == State.FiringArm)
            {
                seeker.StartPath(rb.position, rb.position, OnPathComplete);
            }
            if (state == State.Run)
            {
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
            }
            if (state == State.Fold)
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
            if (state == State.Roam)
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
    void FixedUpdate()
    {
        updatePhase();
        updateState();
        CheckLife();

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
        Vector2 Velocity = new Vector2(direction.x * MovementSpeed, direction.y * MovementSpeed);
        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();
        if (state == State.Run)
        {
            animator.SetBool("IsRunning", true);
        }
        else if (state == State.Slam)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsSlamming", true);
        }
        else if (state == State.FiringArm)
        {
            //rb.velocity = new Vector3(0, 0, 0);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsFiringArm", true);
        }
        if (state == State.Idle)
        {
            rb.velocity = new Vector3(0, 0, 0);
            animator.SetBool("IsRunning", false);
        }

        if (state == State.Fold)
        {
            if (IsFolded)
                Velocity = new Vector2(direction.x * MovementSpeed * 5, direction.y * MovementSpeed * 5);
            else
                Velocity = new Vector2(0, 0);
            animator.SetBool("IsFolding", true);
            animator.SetBool("IsRunning", false);
            normalizeLaser = false; // 
        }
        if (state == State.Laser)
        {

            animator.SetBool("IsFolding", false);
            animator.SetBool("IsLasering", true);
            animator.SetBool("IsSummoning", true);
            if (DoneSummoningPillars)
            {
                animator.SetBool("IsSummoning", false);
            }

        }
        if (state == State.Roam)
        {
            IsLasering = false;
            IsSummoning = true;
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

    void updatePhase()
    {

        if ((Hp > (MaxHP * 0.3)) && (Hp < (MaxHP * 0.6)))
        {
            phase = Phase.SeccondPhase;
            if (switchPhase2 == false)
            {
                //state = State.Idle;
                switchPhase2 = true;
            }


        }
        else if (Hp <= MaxHP * 0.3)
        {
            phase = Phase.LastPhase;
        }
    }
    void updateState()
    {

        if (phase == Phase.FirstPhase)
        {
            //AttackType(1);
            //if (PullTimer == PullCooldown)
            //{
            //    AttackType(4);
            //    if (PullTimer != PullCooldown)
            //        AttackType(1);

            //}
            AttackType(1);
        }
        else if (phase == Phase.SeccondPhase)
        {
            if (!animator.GetBool("IsSlamming") && RollnLaserTimer == RollnLaserCooldown && state != State.Laser)
            {
                state = State.Fold;
                AttackType(2);
            }
            AttackType(3);
            if (!animator.GetBool("IsLasering") && !animator.GetBool("IsFolding") && RollnLaserTimer != RollnLaserCooldown)
            {
                if (PullTimer != PullCooldown)
                    AttackType(1);
                else AttackType(4);
            }
            else animator.SetBool("IsSlamming", false);

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
        if (Hp <= 0)
        {
            collider2D.enabled = false;
            animator.SetBool("IsDying", true);
            DropCoins();
            EndStagePopUp.SetActive(true);

        }
    }
    private Vector3 GetRoamingPosition()
    {
        return StartingPosition + Utilities.GetRandomDir() * Random.Range(10f, 7f);
    }
    public void Slam()
    {
        animator.SetBool("IsSlamming", false);
        float scalar = 0.3f;
        Vector2 KnockBack = new Vector2(direction.x * scalar, direction.y * scalar);
        Transform GolemSlam = Instantiate(_slamPrefab, _slamPoint.position, Quaternion.identity);
        GolemSlam.GetComponent<Slam>().SetUp(PlayerController.instance.MaxHP * 0.5f, KnockBack);
        if (pulling) //pull slam has a different color and much higher damage 
        {
            pulling = false;
            GolemSlam.GetComponent<SpriteRenderer>().material.color = Color.cyan;   //No idea why it's that color
            GolemSlam.GetComponent<Slam>().SetUp(PlayerController.instance.MaxHP * 0.9f, KnockBack);
        }


        if ((target.position.x - transform.position.x > 0 && FacingRight == true) || (target.position.x - transform.position.x < 0 && FacingRight == false))
        {
            flip();
            GolemSlam.Rotate(0f, 0f, 180f);
        }
        state = State.Idle;
        InvokeRepeating("SlamCDTimer", 0f, Time.fixedDeltaTime);
    }
    private void FireArmProj()
    {
        if ((target.position.x - transform.position.x > 0 && FacingRight == true) || (target.position.x - transform.position.x < 0 && FacingRight == false))
        {
            flip();
        }
        animator.SetBool("IsFiringArm", false);
        Vector3 aimDirection = (target.position - _armLaunchPoint.position).normalized;
        Transform firedProjectile = Instantiate(_armProjectilePrefab, _armLaunchPoint.position, Quaternion.identity);
        firedProjectile.localScale = firedProjectile.localScale*transform.localScale.x;
        firedProjectile.GetComponent<GolemArm>().setUp(aimDirection, _pullPoint);
        state = State.Slam;
        animator.SetBool("IsSlamming", true);
        pulling = true;
        InvokeRepeating("PullCDTimer", 0f, Time.fixedDeltaTime);
    }


    private void Roll()
    {
        if (!EndOfFold)
        {
            if (Vector3.Distance(transform.position, wayPoint01.position) <= 1.3f && toWayPoint == 1)
            {
                toWayPoint++;
            }
            if (Vector3.Distance(transform.position, wayPoint02.position) <= 1.3f && toWayPoint == 2)
            {
                toWayPoint++;
            }
            if (Vector3.Distance(transform.position, wayPoint03.position) <= 1.3f && toWayPoint == 3)
            {
                toWayPoint++;
            }
            if (Vector3.Distance(transform.position, wayPoint04.position) <= 1.3f && toWayPoint == 4)
            {
                toWayPoint++;
            }
            if (Vector3.Distance(transform.position, wayPoint05.position) <= 1.3f && toWayPoint == 5)
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
    private void Lasering()
    {
        if (state == State.Laser && DoneSummoningPillars)
        {

            if (IsSummoning && !IsLasering)
            {
                List<Transform> temp = new List<Transform>(WayPoints);
                temp.RemoveAt(temp.Count - 1);
                foreach (Transform Position in temp)
                {
                    SummonningPillars(Position);
                }
                IsSummoning = false;
            }
            else if (IsLasering)
            {

                AccessChildLaser.StartLaser();
                if (!normalizeLaser)
                {
                    AccessChildLaser.transform.eulerAngles = new Vector3(0, 0, 0); // called only once in this state
                    normalizeLaser = true;
                }
                if (AccessChildLaser.endOfLaser)
                {
                    IsFolded = false;
                    //MovementSpeed = _baseMoveSpeed;
                    state = State.Roam;
                    AccessChildLaser.endOfLaser = false;
                    AccessChildLaser.start = false;
                    InvokeRepeating("RollandLaserCDTimer", 0f, Time.fixedDeltaTime);   //Laser cooldown 
                }

            }
        }
    }

    private void SummonningPillars(Transform Position)
    {
        Transform Pillars = Instantiate(_pillarsPrefab, Position.position, Quaternion.identity);


    }
    public void HasFolded()
    {
        IsFolded = true;
        MovementSpeed = _baseMoveSpeed * 1.3f;
    }
    public void InLaseringAnimation()
    {
        IsLasering = true;
    }
    public void Summoning()
    {
        DoneSummoningPillars = true;
    }
    private void AttackType(int phase)
    {
        switch (phase)
        {
            case 1:
                double AttackRange = 5;
                if (Vector3.Distance(transform.position, target.transform.position) <= AttackRange && SlamTimer == SlamCooldown)
                {
                    state = State.Slam;
                }
                else
                {
                    state = State.Run;
                }
                break;
            case 2:
                Roll();
                break;
            case 3:
                //Laser
                Lasering();
                break;
            case 4:
                //Pull here
                double FireRange = 20;
                double SlamRange = 5;
                if (Vector3.Distance(transform.position, target.transform.position) <= FireRange
                    && Vector3.Distance(transform.position, target.transform.position) > SlamRange
                    && PullTimer == PullCooldown
                    && SlamTimer == SlamCooldown)
                {
                    state = State.FiringArm;
                }
                else
                {
                    state = State.Run;
                }
                break;
            default:
                break;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Golem touch player butt");
            Vector3 KnockBackDir = (transform.position - PlayerController.instance.transform.position).normalized;
            KnockBackDir.z = 0;
            PlayerController.instance.takeDamage(Damage * 0.25f, DamageType.Physical, KnockBackDir);
        }
    }
    private void SlamCDTimer()
    {
        SlamTimer -= Time.fixedDeltaTime;
        if (SlamTimer <= 0)
        {
            SlamTimer = SlamCooldown;
            CancelInvoke("SlamCDTimer");
        }

    }
    private void RollandLaserCDTimer()
    {
        RollnLaserTimer -= Time.fixedDeltaTime;
        if (RollnLaserTimer <= 0)
        {
            RollnLaserTimer = RollnLaserCooldown;
            CancelInvoke("RollandLaserCDTimer");
        }

    }
    private void PullCDTimer()
    {
        PullTimer -= Time.fixedDeltaTime;
        if (PullTimer <= 0)
        {
            PullTimer = PullCooldown;
            CancelInvoke("PullCDTimer");
        }
    }
    

}
