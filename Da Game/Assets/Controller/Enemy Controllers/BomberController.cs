using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BomberController : Bomber
{
    
    private BoxCollider2D boxCollider2D;
    private Vector2 direction;
    private Vector3 StartingPosition;
    private Vector3 RoamPosition;
    public float nextWaypointDistance = 3f;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;


    Seeker seeker;

    bool FacingRight = false;

    // Start is called before the first frame update
    private void Start()
    {
        
        target = PlayerController.instance.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        StartingPosition = transform.position;
        InvokeRepeating("UpdatePath", 0f, .5f);
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
            boxCollider2D.enabled = false;
            Drop();
            animator.SetBool("IsExploding", true);
            
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

        if (Vector2.Distance(transform.position, target.transform.position) <= AttackRange)
        {
            animator.SetBool("IsExploding",true);

        }
        if (animator.GetBool("IsExploding"))
            Velocity = new Vector2(0, 0);

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
    private void Explode()
    {
        Vector3 scaleVector = new Vector3(1.2f,1.2f,0);
        Vector3 offset = new Vector3(0,1f,0);
        Transform Explosion = Instantiate(explodePrefab,transform.position+ offset,Quaternion.identity);
        Explosion.GetComponent<ExplosionController>().SetUp(Damage,scaleVector);
    }
}
