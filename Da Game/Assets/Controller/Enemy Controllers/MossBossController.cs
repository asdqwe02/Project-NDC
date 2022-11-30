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
    [SerializeField] bool spinAttackCD;
    [Header("Skill Queue")]
    Queue<int> attackTypeQueue;
    public enum State
    {
        Attack,
        Spell,
        Teleport,
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
        attackRange.x *= GetComponent<SpriteRenderer>().bounds.size.x;
        attackRange.y *= GetComponent<SpriteRenderer>().bounds.size.y / 2.25f;
        collider2D = GetComponent<CapsuleCollider2D>();

    }
    private void Start()
    {
        target = PlayerController.instance.transform;

        attackRange = new Vector2(2f, 1f);
        MaxHP = Hp;

        state = State.Run;
        InvokeRepeating("UpdatePath", 0f, .5f);
        StartCoroutine(SpinAttack(10));

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
            // }
            if (!spinAttackCD)
            {
                StartCoroutine(SpinAttack(10f));
            }

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
            if (animator.GetBool("Attack"))
            {
                Vector2 knockBack = (PlayerController.instance.transform.position - transform.position).normalized;

            }
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
        spinAttackCD = true;
        animator.SetInteger("AttackType", 0);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(duration);
        animator.SetTrigger("EndSpinAttack");
        yield return CoolDown(
             _spinAttackCDTime,
             () => { spinAttackCD = true; },
             () => { spinAttackCD = false; }
         );
    }
}

