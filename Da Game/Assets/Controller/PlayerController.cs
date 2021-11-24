using UnityEngine;

public class PlayerController : PlayerClass
{
    public static bool IsLoading; //1: load , 2 : create
    public int Money;
    public int UnlockedSlot;
    public static string slot;
    //Variables used in Shooting 
    private Vector3 _lookDirection;

    //Variables used in Movement
    private Vector2 _moveDirection;
    [SerializeField] BoxCollider2D collider2D;
    private bool IsHurt;
    private float ImmuneTime = 2;
    private bool isDashButtonDown;
    private float MaxHealth;
    private float _rotationSpeed;
    public bool FacingRight;

    public float Counter = 0;  //Making a counter to wait until the player can go back to sleep if it's in the holding gun state
    public float waitTime = 4;
    [SerializeField] bool _restrictMovement = false;
    public static PlayerController instance;
    public Animator animator;
    private float rotZ;
    private Vector3 difference;

    public static PlayerController Singleton;
    [Header("Layer Masks")]
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private LayerMask _enemyLayerMask;

    [Header("Attack Point")]
    [SerializeField] private Transform _meleeAttackPoint;
    [SerializeField] private Transform _meleeSlashEffectPoint;
    [SerializeField] private Transform _fireAttackPoint; //Haven't used this yet will use later

    [Header("Miscellaneous")]
    [SerializeField] private GameObject interactIcon;
    private Vector2 Size = new Vector2(0.1f, 0.1f);


    private void Awake()
    {
        if (IsLoading)
        {
            PlayerData data = SaveSytemManagement.LoadPlayer(slot);
            if (data != null)
                Load(data);
        }
        //save 
        SaveSytemManagement.SavePlayer(slot, this);

        Singleton = this;

        FiringTime -= Time.deltaTime;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        MaxHealth = Hp;
        IsPlayer = true;
        Money = 0;
        collider2D = GetComponent<BoxCollider2D>();
        UnlockedSlot = 0;


    }
    // Update is called once per frame
    void Update()
    {
        if (!_restrictMovement)
            ProcessInput();
    }

    private void FixedUpdate()
    {
        if (!_restrictMovement)
            Move();
        if (isDashButtonDown == true)
        {
            Dash();
        }
        else if (Physics2D.GetIgnoreLayerCollision(9, 8) && !isDashButtonDown && !IsHurt)
        {
            Physics2D.IgnoreLayerCollision(9, 8, false);
        }

    }

    void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ApplyStatusEffect(DamageType_);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ApplyStatusEffect(DamageType_);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ApplyStatusEffect(DamageType_);
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteraction();
        }
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

        //Melee
        if (Input.GetMouseButtonDown(1) && !animator.GetBool("ToSleep"))
            MeleeAttack();

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && _moveDirection.magnitude != 0)
        {
            isDashButtonDown = true;
        }

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

        if (Counter >= waitTime)         //signal the animator to put the character back to sleep state
        {
            animator.SetBool("ToSleep", true);
        }
        else
        {
            animator.SetBool("ToSleep", false);
        }
        if (IsHurt)
        {
            ImmuneTime -= Time.deltaTime;
            if (ImmuneTime <= 0)
            {
                //collider2D.enabled = true;
                animator.SetBool("IsHurt", false);
                Physics2D.IgnoreLayerCollision(9, 8, false);
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
        Vector3 barrelPos = _fireAttackPoint.position;

        _lookDirection = ((Vector3)vecTemp - transform.position).normalized;

        //this is very dumb and will need a default bullet type TODO: assign a default bullet 
        Transform bulletType = GetBulletType();
        Transform firedBullet = Instantiate(bulletType, barrelPos, Quaternion.identity);

        //Replace DamageType enum with a variable damageType later
        firedBullet.GetComponent<Bullet>().setUp(_lookDirection, true, Damage,DamageType_);
    }
    private void FireBulletSpreadMode()
    {
        //Barrel
        Vector3 barrelPos = _fireAttackPoint.position;

        float startAngle = 90f, endAngle = 270f;
        if (!FacingRight)
        {
            startAngle = 270f;
            endAngle = 90f;
        }

        float angleStep = (endAngle - startAngle) / BulletAmount;
        float angle = startAngle;
        Vector2 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //this is very dumb and will need a default bullet type TODO: assign a default bullet 
        Transform bulletType = GetBulletType();
        for (int i = 0; i < BulletAmount; i++)
        {
            //Stable Spread Fire v1
            float burDirX = barrelPos.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float burDirY = barrelPos.y + Mathf.Cos((angle * Mathf.PI) / 180f);
            Vector3 bulleDirVector = new Vector3(burDirX, burDirY, 0f);

            //Test
            vecTemp.x += Mathf.Sin((angle * Mathf.PI) / 180f);
            vecTemp.y += Mathf.Cos((angle * Mathf.PI) / 180f);
            //Test
            _lookDirection = ((Vector3)vecTemp - bulleDirVector).normalized;

            Transform firedBullet = Instantiate(bulletType, barrelPos, Quaternion.identity);

            //Replace DamageType enum with a variable damageType later
            firedBullet.GetComponent<Bullet>().setUp(_lookDirection, true, Damage, DamageType_);
            angle += angleStep;
        }

    }
    //Get bullet type function
    private Transform GetBulletType()
    {
        try
        {
            switch (DamageType_)
            {
                case DamageType.Physical:
                    break;
                case DamageType.Fire:
                    return BulletPrefab[1];
                    break;
                case DamageType.Cold:
                    return BulletPrefab[2];
                    break;
                case DamageType.Lightning:
                    return BulletPrefab[3];
                    break;
                default:
                    break;
            }
        }
        catch (System.ArgumentNullException)
        {
            
            throw;
        }
        return null; //If damage type is not found throw null exception and return null
    }
    private void Move()
    {
        Rb.velocity = new Vector2(_moveDirection.x * MovementSpeed, _moveDirection.y * MovementSpeed);

        if ((FacingRight && (rotZ < -89 || rotZ > 89)) || (!FacingRight && (rotZ > -89 && rotZ < 89)))
            Flip();



    }
    private void Dash()
    {
        Physics2D.IgnoreLayerCollision(9, 8);
        Vector3 beforeDashPosition = transform.position;
        Vector3 dashPos = transform.position + (Vector3)_moveDirection * DashRange;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _moveDirection, DashRange, _dashLayerMask);
        if (raycastHit2D.collider != null)
        {
            dashPos = raycastHit2D.point;
        }
        Rb.MovePosition(dashPos);

        Transform dashEffectTransform = Instantiate(DashPrefab, beforeDashPosition, Quaternion.identity);
        Vector3 Temp = new Vector3(0, 0, Ultilities.GetAngleFromVectorFloat(_moveDirection));
        dashEffectTransform.eulerAngles = new Vector3(0, 0, Ultilities.GetAngleFromVectorFloat(_moveDirection));
        if (Temp.z > 89 || Temp.z < -89)
        {
            dashEffectTransform.Rotate(0f, 180f, 180f);
        }
        float DashEffectWidth = 3.5f;
        dashEffectTransform.localScale = new Vector3(DashRange / DashEffectWidth, 1f, 1f);
        isDashButtonDown = false;
    }
    //Melee attack function TODO:Add Cooldown
    private void MeleeAttack()
    {
        Transform meleeSlashEffectTransform = Instantiate(MeleeSlashPrefab, _meleeSlashEffectPoint.position, Quaternion.identity);
        if (FacingRight)
            meleeSlashEffectTransform.Rotate(0f, 0f, 224.2f);
        else meleeSlashEffectTransform.Rotate(0f, 180f, 224.2f);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_meleeAttackPoint.position, MeleeAttackRange, _enemyLayerMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            MovingObjects Monster = enemy.GetComponent<MovingObjects>();
            Debug.Log("Hit" + enemy.name);
        }

    }
    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
        interactIcon.transform.Rotate(0f, 180f, 0f);
    }

    //obsolete delete later PLEASE DO NOT USE THIS
    public void setBuff(Buff buff)
    {
        if (string.IsNullOrEmpty(buff.getBuffType()))
        {
            // movementSpeed += buff.getSpeedInc();
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

    public override void takeDamage(float damage, DamageType damageType, Vector2 KnockBack)
    {
        base.takeDamage(damage, damageType, KnockBack);
        Rb.AddForce(KnockBack);
        animator.SetBool("IsHurt", true);
        IsHurt = true;
        ImmuneTime = 3;
        Physics2D.IgnoreLayerCollision(9, 8);
    }

    /*remove the block bellow if the takeDamage function above work*/

    //public override void takeDamage(float damage, Vector2 KnockBack)
    //{

    //    base.takeDamage();
    //    Hp -= damage;
       
    //    //collider2D.enabled = false
      
    //}

    /*Use this to show the melee attack range*/
    private void OnDrawGizmosSelected()
    {
        if (_meleeAttackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_meleeAttackPoint.position, MeleeAttackRange);
    }
    public void SetRestrictMovement(bool restrict)
    {
        _restrictMovement = restrict;
    }
    public void SetVelocity(Vector2 v)
    {
        Rb.velocity = v;
    }

    //Interact
    public void OpenInteractableIcon()
    {
        interactIcon.SetActive(true);
    }
    public void CloseInteractableIcon()
    {
        interactIcon.SetActive(false);
    }
    private void CheckInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Size, 0, Vector2.zero);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
    }

    //Do we even need these get Hp method ??? (Thien)
    public float GetHealth()
    {
        return Hp;
    }
    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void Load(PlayerData data)
    {
        UnlockedSlot = data.UnlockedSlots;
        Money = data.Money;
    }

    public void Save()
    {
        SaveSytemManagement.SavePlayer(slot, this);
    }
}

