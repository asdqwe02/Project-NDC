using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables used in Shooting 
    private Vector3 _lookDirection;
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _firingTime = 0f;
    [SerializeField] private int _bulletAmount = 3;
    [SerializeField] private int _fireType = 0;

    //Variables used in Movement
    private Vector2 moveDirection;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _dashRange;
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private Transform _DashPrefab;

    [SerializeField] private string _buff = "";

    private bool isDashButtonDown;
    public Rigidbody2D rb;
    private float _rotationSpeed;
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
        _firingTime -= Time.deltaTime;
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

        moveDirection = new Vector2(moveX, moveY).normalized;


        //Firing
        if (_firingTime > 0)
            _firingTime = _firingTime - Time.deltaTime;
        else
        {
            if (Input.GetMouseButton(0) && !animator.GetBool("ToSleep"))
            {
                _firingTime = _firingTime + _fireRate;
                switch (_fireType)
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
        if (Input.GetKeyDown(KeyCode.Space))
            isDashButtonDown = true;

        //Update Animation When Moving
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
        //_lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        Transform firedBullet = Instantiate(_bulletPrefab, barrelPos, Quaternion.identity);
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
        float angleStep = (endAngle - startAngle) / _bulletAmount;
        float angle = startAngle;
        Vector2 vecTemp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < _bulletAmount; i++)
        {

            float burDirX = barrelPos.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float burDirY = barrelPos.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulleDirVector = new Vector3(burDirX, burDirY, 0f);
            _lookDirection = ((Vector3)vecTemp - bulleDirVector).normalized;
            Transform firedBullet = Instantiate(_bulletPrefab, barrelPos, Quaternion.identity);
            firedBullet.GetComponent<Bullet>().setUp(_lookDirection);
            angle += angleStep;
        }

    }
    private void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * _movementSpeed, moveDirection.y * _movementSpeed);

        if ((FacingRight && (rotZ < -89 || rotZ > 89)) || (!FacingRight &&(rotZ >-89 && rotZ<89)) )
            Flip();



    }
    private void Dash()
    {
        Vector3 beforeDashPosition = transform.position;
        Vector3 dashPos = transform.position + (Vector3)moveDirection * _dashRange;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDirection, _dashRange,_dashLayerMask);
        if (raycastHit2D.collider != null)
        {
            dashPos = raycastHit2D.point;
        }
        rb.MovePosition(dashPos);

        Transform dashEffectTransform = Instantiate(_DashPrefab, beforeDashPosition, Quaternion.identity);
        dashEffectTransform.eulerAngles = new Vector3(0, 0, Ultilities.GetAngleFromVectorFloat(moveDirection));
        float DashEffectWidth = 3.5f;
        dashEffectTransform.localScale = new Vector3(_dashRange / DashEffectWidth, 1f, 1f);


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
            _movementSpeed += buff.getSpeedInc();
        }
        else
        {
            _buff = buff.getBuffType();
            switch (buff.getBuffType())
            {
                case "Spread":
                    _fireRate = 0.7f;
                    _fireType = 1;
                    break;
                case "SingleLine":
                    _fireType = 0;
                    _fireRate = 0.05f;
                    break;
                default:
                    break;
            }
        }

    }

}
