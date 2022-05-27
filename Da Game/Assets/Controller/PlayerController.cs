using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : PlayerClass
{
    public static bool IsLoading; //1: load , 2 : create
    public static bool To_Load = false;
    public int UnlockedSlot;
    public static string slot;
    //Variables used in Shooting 
    private Vector2 _lookDirection;
    public float meleeAttackTime;
    //Variables used in Movement
    private Vector2 _moveDirection;
    private double DashReset_TakeTooLong = 0.75;
    private bool CanDash = true;
    private int HasDashCounter = 0;
    private double DashCoolDown = 0.75;
    [SerializeField] BoxCollider2D collider2D;
    private bool IsHurt;
    private double ImmuneTime = 0.75;
    private bool isDashButtonDown;
    private float _rotationSpeed;
    public bool FacingRight;

    public float Counter = 0;  //Making a counter to wait until the player can go back to sleep if it's in the holding gun state
    public float waitTime = 4;
    [SerializeField] bool _restrictMovement = false;
    public static PlayerController instance;
    public Animator animator;
    private float rotZ;
    private Vector3 difference;

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
    public float Coin_tobeAdded = 0;
    public bool Death = false;
    public bool InHO = true;

    private Scene scene;

    private void Awake()
    {
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
        FiringTime -= Time.deltaTime; // reset firing time
        meleeAttackTime -= Time.deltaTime;
        BaseDamage = Damage; // setup base damage
        FireRate = 1/AttackSpeed;
        BaseAttackSpeed = AttackSpeed; // setup base attack speed
       
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (IsLoading)
        {
            PlayerData data = SaveSytemManagement.LoadPlayer(slot);
            if (data != null)
            {

                UnlockedSlot = data.UnlockedSlots;
                coins = data.coins;
            }
        }
        else
        {
            coins = 0;
            UnlockedSlot = 0;
        }
        //save 
        SaveSytemManagement.SavePlayer(slot, this);
        MaxHP = Hp;
        IsPlayer = true;

        MaxHP = Hp;

        IsPlayer = true;
        collider2D = GetComponent<BoxCollider2D>();
        Save_Base();
    }



    // Update is called once per frame
    void Update()
    {
        CheckHP();
        if (!_restrictMovement && !statusEffects.Contains(StatusEffect.Freeze))
            ProcessInput();
    }

    private void FixedUpdate()
    {
        if (To_Load)
            Load();
        if (!_restrictMovement && !statusEffects.Contains(StatusEffect.Freeze))
            Move();
        if (statusEffects.Contains(StatusEffect.Freeze))
        {
            Rb.velocity = Vector2.zero;
            animator.SetBool("IsRunning", false);

        }
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

        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    ApplyStatusEffect(10, DamageType.Fire); //damage taken is 10 for tessting purpose
        //}
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    ApplyStatusEffect(10, DamageType.Cold);
        //}
        //if (Input.GetKeyDown(KeyCode.F3))
        //{
        //    ApplyStatusEffect(10, DamageType.Lightning);
        //}


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

                AudioManager.instance.PlaySound(AudioManager.Sound.PlayerShoot);
                FiringTime = FiringTime + FireRate;
                switch (FireType)
                {
                    case 0:
                        FireBullet();
                        break;
                    case 1:
                        //FireBulletSpreadMode();
                        FireBulletSpreadV2(); //Machine god control this not me 
                        break;
                    case 2:
                        FireBulletSpreadModeV3();
                        break;
                    default:
                        break;
                }
            }
        }

        //Melee
        if (meleeAttackTime > 0)
            meleeAttackTime = meleeAttackTime - Time.deltaTime;
        else if (Input.GetMouseButtonDown(1) && !animator.GetBool("ToSleep"))
        {
            MeleeAttack();
            meleeAttackTime += 1/AttackSpeed;
        }

        //Dash
        if (CanDash)
        {
            if (HasDashCounter == 1)
            {
                DashReset_TakeTooLong -= Time.deltaTime;
                if (DashReset_TakeTooLong <= 0)
                {
                    DashReset_TakeTooLong = 0.75;
                    HasDashCounter = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && _moveDirection.magnitude != 0)
            {
                isDashButtonDown = true;
                HasDashCounter += 1;
                if (HasDashCounter == 2)
                {
                    CanDash = false;
                    DashCoolDown = 0.75;
                    DashReset_TakeTooLong = 0.75;
                }
            }
        }
        else
        {
            DashCoolDown -= Time.deltaTime;
            if (DashCoolDown <= 0)
            {
                CanDash = true;
                HasDashCounter = 0;
            }
        }

        //Update Animation When Moving
        if (_moveDirection.magnitude != 0)
        {
            animator.SetBool("IsRunning", true);        //check whether the player is running with its magnitude
            //AudioManager.instance.PlaySound(AudioManager.Sound.SlimeMoving); //Testing moving sound
            Counter = 0; //Reset the timer
        }
        else if (Counter < waitTime)
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
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //mouse position ,normalize with angle comparing to the player
    }

    private void FireBullet()
    {
        Vector3 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vecTemp.z = transform.position.z;
        //this is very dumb and will need a default bullet type TODO: assign a default bullet 
        Transform bulletType = GetBulletType();
        //Barrel
        Vector3 barrelPos = _fireAttackPoint.position;

        _lookDirection = (vecTemp - transform.position).normalized;


        Transform firedBullet = Instantiate(bulletType, barrelPos, Quaternion.identity);
        firedBullet.GetComponent<Bullet>().setUp(_lookDirection, true, Damage, DamageType_);
    }

    //Spread fire version 2 this is control by the machine god idk how it work exactly 
    private void FireBulletSpreadV2()
    {
        //Barrel
        Vector3 barrelPos = _fireAttackPoint.position;
        Transform bulletType = GetBulletType();

        Vector3 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vecTemp.z = transform.position.z;
        _lookDirection = (vecTemp - transform.position).normalized;
        float spreadAngle = 5f * (float)BulletAmount; //5 is the magiic number that make it work don't ask why
        float startRotation = spreadAngle / 2f; //might be something wrong with this  Utilities.GetAngleFromVectorFloatWF(_lookDirection) + 
        float angleIncrease = spreadAngle / ((float)BulletAmount - 1f);
        //Debug.Log("Spread Angle Increase = " + angleIncrease);


        for (int i = 0; i < BulletAmount; i++)
        {
            float tempRota;
            Vector3 spreadDirection;
            tempRota = startRotation - angleIncrease * i; // minus to rotate counter clockwise and plus to rotate clockwise

            /*this rotate the vector by tempRota degree but it seem like it also flip the vecotr around so 
                I use -_lookDirection instead... BUT with an odd amount of bullet amount it's reverse... idk why
            BUT also the spreadAngle doesn't work right when there are an odd amount of bullet */
            spreadDirection = Utilities.RotateA2DVector(-_lookDirection, tempRota);

            Transform firedBullet = Instantiate(bulletType, barrelPos, Quaternion.identity);

            //if (i == 0)
            //    firedBullet.GetComponent<SpriteRenderer>().color = Color.red; //this is used for debugging 
            firedBullet.GetComponent<Bullet>().setUp(spreadDirection, true, Damage, DamageType_);
        }
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
        _lookDirection = ((Vector3)vecTemp - transform.position).normalized;

        //Spread Fire v2 this doesn't work yet 
        //Rad2Deg convert Atan which use randians (Rad) to degree so we can use it easier 
        //float spreadAngle = 270f;
        //_lookDirection = ((Vector3)vecTemp - transform.position).normalized;
        //float playerFacingRotation = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        //float startRotation = playerFacingRotation + spreadAngle / 2f;
        //float angleIncrease = spreadAngle / ((float)BulletAmount - 1f);


        //this is very dumb and will need a default bullet type TODO: assign a default bullet 
        Transform bulletType = GetBulletType();
        for (int i = 0; i < BulletAmount; i++)
        {
            //Spread Fire V2 not stable need tweak nad more MATH
            //float tempRota = startRotation + angleIncrease * i;
            //float buDirX = barrelPos.x + Mathf.Sin(tempRota * Mathf.Deg2Rad);
            //float buDirY = barrelPos.y + Mathf.Cos(tempRota * Mathf.Deg2Rad);
            //Vector3 bulleDirVector = new Vector3(buDirX, buDirY, 0f);
            //vecTemp.x += Mathf.Sin(tempRota * Mathf.Deg2Rad);
            //vecTemp.y += Mathf.Cos(tempRota * Mathf.Deg2Rad);
            //_lookDirection = ((Vector3)vecTemp - bulleDirVector).normalized;

            //Stable Spread Fire v1
            float buDirX = barrelPos.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float buDirY = barrelPos.y + Mathf.Cos((angle * Mathf.PI) / 180f);
            Vector3 bulleDirVector = new Vector3(buDirX, buDirY, 0f);

            _lookDirection = ((Vector3)vecTemp - bulleDirVector).normalized;


            Transform firedBullet = Instantiate(bulletType, barrelPos, Quaternion.identity);
            firedBullet.GetComponent<Bullet>().setUp(_lookDirection, true, Damage, DamageType_);
            vecTemp.x += Mathf.Sin((angle * Mathf.PI) / 180f);
            vecTemp.y += Mathf.Cos((angle * Mathf.PI) / 180f);
            angle += angleStep;
        }

    }
    private void FireBulletSpreadModeV3(){
        //Barrel
        Vector3 barrelPos = _fireAttackPoint.position;

        Vector2 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _lookDirection = ((Vector3)vecTemp - barrelPos).normalized;

        Transform bulletType = GetBulletType();
        for (int i = 0; i < BulletAmount; i++)
        {
            float spreadAngle = UnityEngine.Random.Range(-45f,45f);
            Vector2 shootDir = (Quaternion.Euler(0f,0f,spreadAngle) *  _lookDirection).normalized;
            Transform firedBullet = Instantiate(bulletType, barrelPos, Quaternion.identity);
            firedBullet.GetComponent<Bullet>().setUp(shootDir, true, Damage, DamageType_);
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
                    return BulletPrefab[0];
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
        Vector3 Temp = new Vector3(0, 0, Utilities.GetAngleFromVectorFloat(_moveDirection));
        dashEffectTransform.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVectorFloat(_moveDirection));
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
        Vector3 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vecTemp.z = transform.position.z;
        _lookDirection = (vecTemp - transform.position).normalized;
        //Debug.Log("Look direction magnitude: " + _lookDirection.magnitude);
        foreach (Collider2D enemy in hitEnemies)
        {

            //for dummy
            if (enemy.CompareTag("Dummy"))
            {
                DummyController dummy = enemy.GetComponent<DummyController>();
                dummy.takeDamage(0, DamageType.Physical);
                return;
            }
            if (!enemy.CompareTag("Golem"))
            {
                
                if (enemy.GetComponent<ShieldEnemy>()!=null) // extremely stupid
                    if (enemy.GetComponent<ShieldEnemy>().Shield.activeSelf)
                        return;
                Enemy Monster = enemy.GetComponent<Enemy>();
                Monster.takeDamage(0, DamageType.Physical, _lookDirection);
            }


            //Debug.Log("Hit" + enemy.name);
        }

    }
    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
        interactIcon.transform.Rotate(0f, 180f, 0f);
    }

    public override void takeDamage(float damage, DamageType damageType, Vector2 KnockBack)
    {
        base.takeDamage(damage, damageType, KnockBack);
        Rb.AddForce(KnockBack);
        animator.SetBool("IsHurt", true);
        IsHurt = true;
        ImmuneTime = 0.75;
        Physics2D.IgnoreLayerCollision(9, 8);
    }

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

    private void CheckHP()
    {
        //
        if (Hp <= 0 && !InHO)
        {
            animator.SetBool("IsDying", true);
            Rb.velocity = Vector2.zero;
            _restrictMovement = true;
            // death sound track is played after die animation
        }
    }

    // don't delete this
    public void HasDied()
    {
        Death = true;
        AudioManager.instance.PlaySoundTrack(AudioManager.SoundTrack.GameOverST);
    }


    public void Load()
    {
        if (IsLoading)
        {
            Load_Base();
            PlayerData data = SaveSytemManagement.LoadPlayer(slot);
            if (data != null)
            {
                UnlockedSlot = data.UnlockedSlots;
                coins = data.coins;
            }
        }
        else
        {
            coins = 0;
            UnlockedSlot = 0;
        }
        //save 
        SaveSytemManagement.SavePlayer(slot, this);
        MaxHP = Hp;

        To_Load = false;
        IsLoading = true;
    }


    public void Load_Base()
    {
        PlayerData data = SaveSytemManagement.LoadPlayer("Base");
        if (data != null)
        {
            MaxHP = data.Hp; ;
            Hp = data.Hp;
            MovementSpeed = data.MS;
            Damage = data.damage;
            Armour = data.Armour;
            FireResistance = data.FireRes;
            ColdResistance = data.ColdRes;
            LightningResistance = data.LightningRes;
            AttackSpeed = data.AS;
            FireRate = data.FireRate;
            FireType = data.FireType;
            BulletAmount = data.BulletAmount;
            DamageType_ = DamageType.Physical;
            InHO = true;
            _restrictMovement = false;
            Death = false;
            animator.SetBool("IsDying", false);
            CleanseEffect();
        }
    }
    public void Save()
    {
        SaveSytemManagement.SavePlayer(slot, this);
    }
    public void Save_Base()
    {
        SaveSytemManagement.SavePlayer("Base", this);
    }

    public void CleanseEffect()
    {
        if (statusEffects.Contains(StatusEffect.Freeze))
        {
            RemoveStatusEffect(2);
        }
        if (statusEffects.Contains(StatusEffect.Burning))
        {
            RemoveStatusEffect(1);
        }
        if (statusEffects.Contains(StatusEffect.Shocked))
        {
            RemoveStatusEffect(3);
        }
    }

    public void CalculateDamage()
    {
        
        Damage = BaseDamage * (1+PercentDamageIncrease);
        if (Damage <=0)
            Damage=1;
    }
    public void CalculateAttackSpeed()
    {
        if (-PercentAttackSpeedIncrease>=1)
        {
            // calculation for reduce attack speed if the percent reduce is higher than 100%
            AttackSpeed = BaseAttackSpeed / (BaseAttackSpeed*(1+ Mathf.Abs(PercentAttackSpeedIncrease)));
        }
        else AttackSpeed = BaseAttackSpeed * (1+PercentAttackSpeedIncrease);

        FireRate = 1/AttackSpeed;
    }

}

