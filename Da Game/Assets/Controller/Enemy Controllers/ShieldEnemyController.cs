using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ShieldEnemyController : ShieldEnemy
{
  
    private CircleCollider2D circleCollider2D;
    private Vector2 direction;
    private Vector3 StartingPosition;
    private Vector3 RoamPosition;
    public float nextWaypointDistance = 3f;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    [SerializeField]
    private float shieldCD = 0f;
    [SerializeField]
    private float shieldTimer = 0f;


    Seeker seeker;

    bool FacingRight = false;

    // Start is called before the first frame update
    private void Start()
    {
        shieldPercent = 0.5f;
        target = PlayerController.instance.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        Shield = transform.GetChild(0).gameObject; // get the shield
        StartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, .5f);
        shieldBaseHP = Hp * shieldPercent;
        shieldHP = shieldBaseHP;
        ActivateShield();
        // Invoke("ActivateShield",3);
        // ActivateShield();


    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            if (state == State.ChaseTarget)
            {
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
            }
            else if (state == State.Roaming)
            {
                seeker.StartPath(rb.position, GetRoamingPosition(), OnPathComplete);
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

    private void FixedUpdate()
    {
        CheckLife();

        FindTarget();

        if (!statusEffects.Contains(StatusEffect.Freeze))
            ProcessAction();
        else
        {
            animator.SetBool("IsRunning", false);
            rb.velocity = Vector2.zero;
        }
    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }



    void CheckLife() //should move this to Enemy class
    {
        if (Hp <= 0)
        {
            //isDying = true;
            circleCollider2D.enabled = false;
            Drop();
            animator.SetBool("IsDying", true);
            
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return StartingPosition + Utilities.GetRandomDir() * Random.Range(10f, 7f);
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

    private void ProcessAction()
    {
        if (path == null)
        {
            return;
        }
        //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
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

        if (animator.GetBool("IsDying")  || animator.GetBool("IsActivatingShield"))
        {
            Velocity = new Vector2(0, 0);
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

        if (shieldHP<=0 && shieldActive)
        {
            DeactivateShield();
        }

        
        
    }
    private void ActivateShield(){
        animator.SetBool("IsActivatingShield",false);
        circleCollider2D.enabled = false;
        Shield.SetActive(true);
        shieldActive = true;
        shieldHP = shieldBaseHP;
    }
    private void DeactivateShield(){
        circleCollider2D.enabled = true;
        Shield.SetActive(false);
        shieldActive = false;
        InvokeRepeating("ShieldCDTimer",0f,Time.fixedDeltaTime);

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            float damage;
            if (Shield.activeSelf)
                 damage = 1.5f*Damage;
            else damage = Damage;
            Vector2 knockBack = (PlayerController.instance.transform.position - transform.position).normalized;
            knockBack *=0.5f;
            other.gameObject.GetComponent<PlayerController>().takeDamage(damage,DamageType_,knockBack);
        }
    }

    private void ShieldCDTimer()
    {
        shieldTimer -= Time.fixedDeltaTime;
        if (shieldTimer <= 0)
        {
            shieldTimer = shieldCD;
            animator.SetBool("IsActivatingShield",true);
            CancelInvoke("ShieldCDTimer");
        }
    }
    public void ShieldTakeDamage(float damage)
    {
        shieldHP -= damage;
    } 
}
