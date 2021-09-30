using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Animator animator;
    private Vector3 _shootDir;
    private bool isMoving = true, flip=false;
    [SerializeField] private float _bulletSpeed = 50f;
    
    public void setUp(Vector3 shootDir)
    {
        _shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
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
       
        
        Slime slime = collision.GetComponent<Slime>();
        PlayerController p = collision.GetComponent<PlayerController>();
        Bullet otherBullets = collision.GetComponent<Bullet>();
        if (p != null || otherBullets != null)
        {
            return;
        }
        else
        {
            animator.SetBool("Hit", true);
            isMoving = false;
        }
            
        if (slime != null)
        {
            slime.takeDamage(1);
            
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
