using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Animator animator;
    private Vector3 _shootDir;
    private Vector3 _knockBack;
    private float _damage;
    private int _damageType;
    private RNG statusEffectRNG;
    private bool isMoving = true, flip=false;
    [SerializeField] private float _bulletSpeed = 50f;
    [SerializeField] private bool _isFromPlayer = false;

    private void Start()
    {
        statusEffectRNG = new RNG();
    }
    public void setUp(Vector3 shootDir, bool IsFromPlayer,float damage,int damageType, Vector3 KnockBack)
    {
        _shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        _isFromPlayer = IsFromPlayer;
        _knockBack = KnockBack;
        if (!_isFromPlayer)
            gameObject.layer = 8;
        _damageType = damageType; 
        Destroy(gameObject, 1f);
    }
    public void setUp(Vector3 shootDir, bool IsFromPlayer, float damage,int damageType)
    {
        _shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        _isFromPlayer = IsFromPlayer;
        if (!_isFromPlayer)
            gameObject.layer = 8;
        _damageType = damageType;
        Destroy(gameObject, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            transform.position += _shootDir * _bulletSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy Monster = collision.GetComponent<Enemy>();
        PlayerController p = collision.GetComponent<PlayerController>();
        Bullet otherBullets = collision.GetComponent<Bullet>();
        if (_isFromPlayer)
        {
            if (p != null || otherBullets != null)
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
                Monster.takeDamage(1);
                bool applyStatus = statusEffectRNG.RollNumber(25f);
                if (applyStatus)
                {
                    Monster.ApplyStatusEffect(_damageType);
                }

            }
        }
        else
        {
            if (Monster != null || otherBullets != null)
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
                Debug.Log("Hit Player");
                p.takeDamage(1,_knockBack);

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
