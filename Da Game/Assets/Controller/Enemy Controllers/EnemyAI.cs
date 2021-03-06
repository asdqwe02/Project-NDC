using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Walking,
        Idle,
        Dead,
    }

    public Transform target;
    public float speed = 400;
    public float nextWaypointDistance = 3f;
    //GFX
    bool FacingRight = true;
    public Animator animator;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    Rigidbody2D rb;


    private void Start()
    {

        target = PlayerController.instance.transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }else
        {
            reachedEndofPath = false;
        }


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] -rb.position).normalized;

        Vector2 Velocity = new Vector2(direction.x * speed, direction.y * speed);
        if(animator.GetBool("IsDying") == true )
        {
            Velocity = new Vector2 (0,0);
        }
        rb.velocity = Velocity;



        if ((direction.x >= 0.01f && FacingRight) || (direction.x <= -0.01f && !FacingRight))
            flip();
        if (direction.magnitude ==0)
        {
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }


}
