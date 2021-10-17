using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemArm : MonoBehaviour
{
    private Vector3 _shootDir;
    private Vector3 _knockBack;
    private Rigidbody2D rb;
    private float _damage;
    private bool isPulling = false, flip = false;
    private Transform _pullToPoint;
    [SerializeField] private float _projectileSpeed = 50f;
    [SerializeField] private float _maxRange = 50f;
    private PlayerController player;
  

    public void setUp(Vector3 shootDir, Transform PullPoint)
    {
        _shootDir = shootDir;
        _pullToPoint = PullPoint;
        rb = GetComponent<Rigidbody2D>();
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(-shootDir));
        rb.velocity = new Vector2(_shootDir.x * _projectileSpeed, _shootDir.y * _projectileSpeed);  //apply velocity to golem arm
        Destroy(gameObject, 2f);

    }
    void FixedUpdate()
    {
        //rb.velocity = (Vector2)(_shootDir * _projectileSpeed);

        if (isPulling)
        {
            if (Vector3.Distance(gameObject.transform.position,_pullToPoint.position) <= 1.5f)
            {
                player.SetRestrictMovement(false);
                player.SetVelocity(new Vector2(0, 0));
                Destroy(gameObject);
            }
            else
            {
                Vector2 pullDir = _pullToPoint.position - player.transform.position;
                rb.velocity = pullDir * _projectileSpeed/20;
                player.SetVelocity(pullDir * _projectileSpeed/20);
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy Monster = collision.GetComponent<Enemy>();
        PlayerController p = collision.GetComponent<PlayerController>();
        Bullet otherBullets = collision.GetComponent<Bullet>();
        if (Monster != null || otherBullets != null)
        {
           return;
        }

        if (p != null)
        {
            Debug.Log("Hit Player");
            isPulling = true;

            player = p;
            p.SetRestrictMovement(true);
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
