using UnityEngine;

public class PlayerController : PlayerClass
{
    //Variables used in Shooting 
    private Vector3 _lookDirection;

    //Variables used in Movement
    private Vector2 _moveDirection;
    
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] BoxCollider2D collider2D;
    private bool IsHurt;
    private float ImmuneTime = 3;
    private bool isDashButtonDown;
   
    private float _rotationSpeed;
    public bool FacingRight ;

    public float Counter = 0;  //Making a counter to wait until the player can go back to sleep if it's in the holding gun state
    public float waitTime = 4;
    public static PlayerController instance;
    public Animator animator;
    private float rotZ;             
    private Vector3 difference;

    public static PlayerController Singleton;
    private void Awake()
    {
        Singleton = this;

        FiringTime -= Time.deltaTime;
        if (instance == null)
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
    private void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Move();
        if(isDashButtonDown == true)
        {
            Dash();
        }
    }

    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveX, moveY).normalized;


        //Firing
        if (FiringTime > 0)
            FiringTime = FiringTime - Time.deltaTime;
        else
        {
            if (Input.GetMouseButton(0) && !animator.GetBool("ToSleep"))
            {
                FiringTime = FiringTime + FireRate;
                switch (FireType)
                {
                    case 0:
                        FireBullet();
                        break;
                    case 1:
                        FireBulletSpreadMode();
                        break;
                    default:
                        break;
                }
            }
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && _moveDirection.magnitude != 0)
            isDashButtonDown = true;

        //Update Animation When Moving
        if (_moveDirection.magnitude != 0)
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
        if(IsHurt)
        {
            ImmuneTime -= Time.deltaTime;
            if(ImmuneTime <=0)
            {
                collider2D.enabled = true;
                animator.SetBool("IsHurt", false);
                IsHurt = false;
            }
        }

        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;                           //mouse position ,normalize with angle comparing to the player
    }

    private void FireBullet()
    {
        Vector2 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Barrel
        Vector3 barrelPos = transform.position;
   
        //Do this bc we don't have separate gun from the body yet
        barrelPos.z = 0;
        if (FacingRight)
            barrelPos.x += 0.8f;
        else barrelPos.x -= 0.8f;
        barrelPos.y -= 0.085f;

        _lookDirection = ((Vector3)vecTemp - transform.position).normalized;
        Transform firedBullet = Instantiate(BulletPrefab, barrelPos, Quaternion.identity);
        firedBullet.GetComponent<Bullet>().setUp(_lookDirection);
    }
    private void FireBulletSpreadMode()
    {
        //Barrel
        Vector3 barrelPos = transform.position;
        //Do this bc we don't have separate gun from the body yet
        barrelPos.z = 0;
        if (FacingRight)
            barrelPos.x += 0.8f;
        else barrelPos.x -= 0.8f;
        barrelPos.y -= 0.085f;

        float startAngle = 90f, endAngle = 270f;
        if (!FacingRight)
        {
            startAngle = 270f;
            endAngle = 90f;
        }

        float angleStep = (endAngle - startAngle) / BulletAmount;
        float angle = startAngle;
        Vector2 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < BulletAmount; i++)
        {
            //Stable Spread Fire v1
            float burDirX = barrelPos.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float burDirY = barrelPos.y + Mathf.Cos((angle * Mathf.PI) / 180f);
            Vector3 bulleDirVector = new Vector3(burDirX, burDirY, 0f);
            
            //Test
            vecTemp.x+= Mathf.Sin((angle * Mathf.PI) / 180f);
            vecTemp.y+= Mathf.Cos((angle * Mathf.PI) / 180f);
            //Test


            _lookDirection = ((Vector3)vecTemp - bulleDirVector).normalized;

            Transform firedBullet = Instantiate(BulletPrefab, barrelPos, Quaternion.identity);
            firedBullet.GetComponent<Bullet>().setUp(_lookDirection);
            angle += angleStep;
        }

    }
    private void Move()
    {
        Rb.velocity = new Vector2(_moveDirection.x * movementSpeed, _moveDirection.y * movementSpeed);

        if ((FacingRight && (rotZ < -89 || rotZ > 89)) || (!FacingRight &&(rotZ >-89 && rotZ<89)) )
            Flip();



    }
    private void Dash()
    {
        Vector3 beforeDashPosition = transform.position;
        Vector3 dashPos = transform.position + (Vector3)_moveDirection * DashRange;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _moveDirection, DashRange,_dashLayerMask);
        if (raycastHit2D.collider != null)
        {
            dashPos = raycastHit2D.point;
        }
        Rb.MovePosition(dashPos);

        Transform dashEffectTransform = Instantiate(DashPrefab, beforeDashPosition, Quaternion.identity);
        Vector3 Temp = new Vector3(0, 0, Ultilities.GetAngleFromVectorFloat(_moveDirection));
        dashEffectTransform.eulerAngles = new Vector3(0, 0, Ultilities.GetAngleFromVectorFloat(_moveDirection));
        if (Temp.z>89 || Temp.z < -89)
        {
            dashEffectTransform.Rotate(0f, 180f, 180f);
        }
        float DashEffectWidth = 3.5f;
        dashEffectTransform.localScale = new Vector3(DashRange / DashEffectWidth, 1f, 1f);


        isDashButtonDown = false;
    }
    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    public void setBuff(Buff buff)
    {
        if (string.IsNullOrEmpty(buff.getBuffType()))
        {
            movementSpeed += buff.getSpeedInc();
        }
        else
        {
            Buff = buff.getBuffType();
            switch (buff.getBuffType())
            {
                case "Spread":
                    FireRate = 0.7f;
                    FireType = 1;
                    break;
                case "SingleLine":
                    FireType = 0;
                    FireRate = 0.05f;
                    break;
                default:
                    break;
            }
        }

    }
    public void takeDamage(float damage, Vector2 KnockBack)
    {
        hp -= damage;
        Rb.position = new Vector2(Rb.position.x + KnockBack.x, Rb.position.y + KnockBack.y);
        collider2D.enabled = false;
        animator.SetBool("IsHurt", true);
        IsHurt = true;
        ImmuneTime = 3;
    }




}
