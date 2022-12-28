using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
public class MossBossController : Enemy
{
    [SerializeField] private Vector2 attackRange;


    private Animator animator;
    Rigidbody2D rb;
    private float MaxHP;
    bool FacingRight = false;
    public Transform target;
    Path path;
    [SerializeField] Seeker seeker;
    int currentWaypoint = 0;
    private Vector2 direction;
    bool reachedEndofPath = false;
    public float nextWaypointDistance = 5f;
    private CapsuleCollider2D collider2D;
    public State state;
    [Header("Skill Cooldown")]
    [SerializeField] float _spinAttackCDTime;
    [SerializeField] float _rangeAttackCDTime;
    [SerializeField] float _superAttackCDTime;
    [SerializeField] float _throwAttackCDTime;
    [SerializeField] bool _spinAttackCD;
    [SerializeField] bool _rangeAttackCD;
    [SerializeField] bool _superAttackCD;
    [SerializeField] bool _throwAttackCD;
    [SerializeField] bool _spinning;
    [Header("Skill Queue")]
    Queue<int> attackTypeQueue;
    public enum State
    {
        Static,
        Buff,
        Run,
        Idle,
        Roam,
        Die
    }
    private void Awake()
    {
        base.Awake();
        if (animator == null)
            animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // attackRange.x *= GetComponent<SpriteRenderer>().bounds.size.x;
        // attackRange.y *= GetComponent<SpriteRenderer>().bounds.size.y / 2.25f;
        collider2D = GetComponent<CapsuleCollider2D>();

    }
    private void Start()
    {
        target = PlayerController.instance.transform;

        attackRange = new Vector2(2f, 1f);
        MaxHP = Hp;

        state = State.Run;
        InvokeRepeating("UpdatePath", 0f, .5f);

    }
    void UpdatePath() // being call in InvokeRepeating above
    {
        if (seeker.IsDone())
            if (state == State.Run)
            {
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
            }
            else seeker.StartPath(rb.position, rb.position, OnPathComplete);
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
        UpdateAction();
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
        if (state != State.Run)
            Velocity = Vector2.zero;
        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();

        switch (state)
        {
            // case State.Run:
            //     animator.SetBool("Run", true);
            //     animator.SetBool("Idle", false);
            //     animator.SetBool("Attack", false);
            //     animator.SetBool("Spell", false);
            //     break;
            // case State.Attack:
            //     animator.SetBool("Attack", true);
            //     animator.SetBool("Spell", false);
            //     animator.SetBool("Run", false);
            //     animator.SetBool("Idle", false);
            //     break;
            // case State.Spell:
            //     animator.SetBool("Spell", true);
            //     animator.SetBool("Attack", false);
            //     animator.SetBool("Run", false);
            //     animator.SetBool("Idle", false);
            //     break;
            // case State.Teleport:
            //     animator.SetBool("Teleport", true);
            //     animator.SetBool("Run", false);
            //     animator.SetBool("Idle", false);
            //     animator.SetBool("Attack", false);
            //     animator.SetBool("Spell", false);
            //     break;
            // case State.Die:
            //     animator.SetBool("Dying", true);
            //     break;
            // default:
            //     Debug.Log("State = null in Bringer Of Death Controller Script!");
            //     break;
        }


        float distance = Vector2.Distance(seeker.transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        rb.velocity = Velocity;
    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void UpdateAction()
    {
        if (state != State.Die)
        {
            // if (Mathf.Abs(target.position.x - transform.position.x) <= attackRange.x
            //     && Mathf.Abs(target.position.y - transform.position.y) <= attackRange.y
            //   )
            // {
            //     state = State.Attack;
            // }\
            if (
                   !_spinAttackCD
                    && state != State.Static
                   && state != State.Buff)
            {
                StartCoroutine(SpinAttack(3f));
            }
            else if ((!_rangeAttackCD || !_superAttackCD || !_throwAttackCD) && !_spinning && animator.GetInteger("AttackType") != 0)
            {
                state = State.Static;
                animator.SetTrigger("Attack");
                switch (animator.GetInteger("AttackType"))
                {
                    case 1:
                    case 2:
                    case 3:
                        break;
                }
            }
            // if (Mathf.Abs(target.position.x - transform.position.x) <= attackRange.x
            //   && Mathf.Abs(target.position.y - transform.position.y) <= attackRange.y)
            // {

            // }

        }

    }
    public void CheckAttackRange()
    {
        // && !Utilities.IsAnimationPlaying(animator,"attack")
        if ((target.position.x - transform.position.x > 0 && FacingRight == true) || (target.position.x - transform.position.x < 0 && FacingRight == false))
        {
            flip();
        }
        if (Mathf.Abs(target.position.x - transform.position.x) > attackRange.x
            || Mathf.Abs(target.position.y - transform.position.y) > attackRange.y)
        {
            state = State.Run;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("BoD hit player with attack");
            // if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            // {
            //     Debug.Log("Hit player layer");
            // }

            Vector2 knockBack = (PlayerController.instance.transform.position - transform.position).normalized;
            PlayerController.instance.takeDamage(1, DamageType.Cold, knockBack);
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player is still in trigger");
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            // Debug.Log("monster touch monster");
            Physics2D.IgnoreCollision(other.collider, collider2D);
        }
        if (other.collider.CompareTag("Player"))
        {
            Vector2 knockBack = 0.3f * (PlayerController.instance.transform.position - transform.position).normalized;
            PlayerController.instance.takeDamage(Damage / 10f, DamageType.Physical, knockBack);
        }
        // Debug.Log("this collider" + other.collider.name);
        // Debug.Log("other collider" + other.otherCollider.name);


    }

    public IEnumerator CoolDown(float CDTime, Action preCDAction = null, Action postCDAction = null)
    {
        preCDAction?.Invoke();
        yield return new WaitForSeconds(CDTime);
        postCDAction?.Invoke();
    }


    public void CheckLife()
    {
        // if (Hp <= 0.6f * MaxHP && !phase[1] && !isClone)
        // {
        //     StartCoroutine(AudioManager.Instance.FadeOutST(fadeDuration: 0.75f, targetVolumne: 0.05f, NextST: AudioManager.SoundTrack.BossST2_Phase2));
        //     // phase[1] = true; // phase 2
        //     AttackSpeed *= 1.25f;
        //     Damage *= 1.5f;
        //     animator.SetFloat("AttackSpeed", AttackSpeed);

        // }

        // if (Hp <= 0 && state != State.Die)
        // {
        //     state = State.Die;


        //     if (!isClone)
        //     {
        //         BringerOfDeathController[] bossClone = GameObject.FindObjectsOfType<BringerOfDeathController>();
        //         foreach (var clone in bossClone)
        //             clone.state = State.Die;
        //         Drop();
        //     }
        // }
    }
    public override void OnDeath(object sender, OnDeathEventArgs e)
    {
        if (state != State.Die)
            state = State.Die;


        Drop();
        DropCoins();

    }
    IEnumerator SpinAttack(float duration)
    {
        _spinAttackCD = true;
        _spinning = true; ;
        animator.SetInteger("AttackType", 0);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(duration);
        animator.SetTrigger("EndSpinAttack");
        _spinning = false;
        CheckNextSkill();
        yield return CoolDown(
             _spinAttackCDTime,
             () => { _spinAttackCD = true; },
             () => { _spinAttackCD = false; }
         );

    }

    public void CheckNextSkill()
    {
        if (!_superAttackCD)
        {
            animator.SetInteger("AttackType", 2);
            return;
        }
        if (!_rangeAttackCD)
        {
            animator.SetInteger("AttackType", 1);
            return;
        }
        if (!_throwAttackCD)
        {
            animator.SetInteger("AttackType", 3);
            return;
        }
        animator.SetInteger("AttackType", 0);
    }
    public void RangeAttackCoolDown()
    {
        StartCoroutine(CoolDown(
            _rangeAttackCDTime,
            () => { _rangeAttackCD = true; },
            () => { _rangeAttackCD = false; }
        ));
        state = State.Run;
    }
    public void SuperAttackCoolDown()
    {
        StartCoroutine(CoolDown(
           _superAttackCDTime,
           () => { _superAttackCD = true; },
           () => { _superAttackCD = false; }
       ));
        state = State.Run;
    }
}

