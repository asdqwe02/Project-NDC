using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private Vector2 moveDirection;
    public float MovementSpeed;
    public Rigidbody2D rb;
    public float RotationSpeed;
    public bool FacingRight ;

    public float Counter = 0;  //Making a counter to wait until the player can go back to sleep if it's in the holding gun state
    public float waitTime = 4;

    public static PlayerController instance;
    public string scenePassword;

    public Animator animator;
    private float rotZ;             
    private Vector3 difference;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;



        if (moveDirection.magnitude != 0)
        {
            animator.SetBool("IsRunning", true);        //check whether the player is running with its magnitude
            Counter = 0; //Reset the timer
        }
        else
        {
            Counter += Time.deltaTime;
            animator.SetBool("IsRunning", false);
        }

        if(Counter >= waitTime)         //signal the animator to put the character back to sleep state
        {
            animator.SetBool("ToSleep", true);
        }
        else
        {
            animator.SetBool("ToSleep", false);
        }


        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;                           //mouse position ,normalize with angle comparing to the player
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * MovementSpeed, moveDirection.y * MovementSpeed);

        if ((FacingRight && (rotZ < -89 || rotZ > 89)) || (!FacingRight &&(rotZ >-89 && rotZ<89)) )
            Flip();



    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }


}
