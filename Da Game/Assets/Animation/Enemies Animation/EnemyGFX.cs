using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyGFX : MonoBehaviour
{
    bool FacingRight = true;
    public AIPath aiPath;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {

        if ((aiPath.desiredVelocity.x >= 0.01f && FacingRight) || (aiPath.desiredVelocity.x <= -0.01f && !FacingRight))
            flip();
        if(aiPath.desiredVelocity.x == 0 && aiPath.desiredVelocity.y == 0)
        {
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
        }

    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
