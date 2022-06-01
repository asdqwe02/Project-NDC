using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BringerOfDeathController : Enemy
{
    private Animator animator;
    Rigidbody2D rb;

    [SerializeField] private Transform spell;
    private int spellAmountPerCast = 15;
    private int castAmount = 2, currentCast=0;
    private float spellOffsetY = 3f;
    

    bool FacingRight = true;


    public Transform target;
    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    private Vector2 direction;
    bool reachedEndofPath = false;
    public float nextWaypointDistance = 3f;



    State state;


    public enum State
    {
        Attack,
        Spell,
        Teleport,
        Run,
        Idle,
        Roam
    }

    private void Start() {

        state = State.Run;

        target = PlayerController.instance.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // direction = new Vector2(0,0);

        if (animator == null)
            animator = GetComponent<Animator>();
        spell.GetComponent<BODSpellController>().SetUp(2f,DamageType.Lightning); // change this later
        spellOffsetY = spell.transform.localScale.y*2.3f;
        currentCast = castAmount;
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

    private void FixedUpdate() {
      

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

        switch (state)
        {
            case State.Run:
                animator.SetBool("Run",true);
                break;
            default:
                Debug.Log("State = null in Bringer Of Death Controller Script!");
                break;
        }


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
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


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("BoD hit player with attack");
            // if (other.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            // {
            //     Debug.Log("Hit player layer");
            // }
            if (animator.GetBool("Attack"))
            {
                Vector2 knockBack = 0.5f*(PlayerController.instance.transform.position - transform.position).normalized;
                PlayerController.instance.takeDamage(Damage,DamageType.Physical,knockBack);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {

        if (other.collider.CompareTag("Player"))
        {
            Vector2 knockBack = 0.3f*(PlayerController.instance.transform.position - transform.position).normalized;
            PlayerController.instance.takeDamage(Damage/10f,DamageType.Physical,knockBack);
        }
        
    }

    public IEnumerator CastSpell()
    {
        // Debug.Log("Bringer of Death spell casted");
        currentCast --;
        if (currentCast == 1)
        {
            animator.SetBool("Spell",false);
            animator.SetBool("Idle",true);
            currentCast = castAmount;
        }
        for (int i = 0; i < spellAmountPerCast; i++)
        {
            Vector3 spawnPos = PlayerController.instance.transform.position;
            spawnPos.y+=spellOffsetY;
            spawnPos.x+= Random.Range(-0.075f,0.075f);
            Instantiate(spell,spawnPos,Quaternion.identity);
            yield return new WaitForSeconds(0.75f);
        }
        
    }
    public IEnumerator Teleport()
    {
        GetComponent<CapsuleCollider2D>().enabled=false; // just a double check
        yield return new WaitForSeconds(3f);
        animator.SetBool("Teleport",false);
        animator.SetBool("Run",true);
        Vector3 teleportPos = PlayerController.instance.transform.position;
        teleportPos.x += Random.Range(-3f,3f);
        transform.position = teleportPos;
    }
}
