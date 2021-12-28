using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Animator animator;
    private Vector3 _shootDir;
    private Vector3 _knockBack;
    private float _damage;
    private MovingObjects.DamageType _damageType;
    private RNG statusEffectRNG;
    private bool isMoving = true, flip = false;
    private Rigidbody2D rb;
    [SerializeField] private float _bulletSpeed = 50f;
    [SerializeField] private bool _isFromPlayer = false;

    private void Start()
    {
        statusEffectRNG = new RNG();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(_shootDir * _bulletSpeed, ForceMode2D.Impulse);


    }
    public void setUp(Vector3 shootDir, bool IsFromPlayer, float damage, MovingObjects.DamageType damageType, Vector3 KnockBack)
    {
        _shootDir = shootDir;
        _damage = damage;
        
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        _isFromPlayer = IsFromPlayer;
        _knockBack = KnockBack;
        if (!_isFromPlayer)
            gameObject.layer = 8;
        _damageType = damageType;
        //rb.AddForce(_shootDir * _bulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 1f);
    }
    public void setUp(Vector3 shootDir, bool IsFromPlayer, float damage, MovingObjects.DamageType damageType)
    {
        _shootDir = shootDir;
        _damage = damage;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        _isFromPlayer = IsFromPlayer;
        if (!_isFromPlayer)
            gameObject.layer = 8;
        _damageType = damageType;
        Destroy(gameObject, 1f);
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            rb.velocity = Vector2.zero;
            //rb.velocity =  _shootDir * _bulletSpeed; // this has a bug need to fix asap

        }
           



    }
    //Bad implementation
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: Change these to compare tag or make an ignore trigger list
        Enemy Monster = collision.GetComponent<Enemy>();
        PlayerController p = collision.GetComponent<PlayerController>();
        Bullet otherBullets = collision.GetComponent<Bullet>();
        BeginWaves bw = collision.GetComponent<BeginWaves>();
        Interactable interactable = collision.GetComponent<Interactable>();
        DummyController dummy = collision.GetComponent<DummyController>();

        if (_isFromPlayer)
        {
            if (p != null || otherBullets != null || bw != null || interactable != null)
            {
                return;
            }
            else
            {
                if (animator != null)
                    animator.SetBool("Hit", true);
                isMoving = false;
            }

            if (Monster != null)
            {
                Monster.takeDamage(_damage, _damageType);
                bool applyStatus = statusEffectRNG.RollNumber(25f); //Apply status effect to monster 
                if (applyStatus)
                {
                    Monster.ApplyStatusEffect(_damage, _damageType);
                }

            }
            if (dummy != null)
            {
                dummy.takeDamage(_damage, _damageType);
                bool applyStatus = statusEffectRNG.RollNumber(25f);
                if (applyStatus)
                {
                    dummy.ApplyStatusEffect(_damage, _damageType);
                }
            }
        }
        else
        {
            if (Monster != null || otherBullets != null || bw != null || interactable != null)
            {
                return;
            }
            else
            {

                if (animator != null)
                    animator.SetBool("Hit", true);
                isMoving = false;
            }
            if (p != null)
            {
                p.takeDamage(_damage, _damageType, _knockBack);
                bool applyStatus = statusEffectRNG.RollNumber(25f); //Apply status effect to player
                if (applyStatus)
                {
                    p.ApplyStatusEffect(_damage, _damageType);
                }

            }
        }  
    }
    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;
        return n;
    }
}
