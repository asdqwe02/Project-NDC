using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _shootDir;
    [SerializeField] private float _bulletSpeed = 50f;
    public void setUp(Vector3 shootDir)
    {
        _shootDir = shootDir;
        Destroy(gameObject, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += _shootDir * _bulletSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        PlayerController p = collision.GetComponent<PlayerController>();
        Bullet otherBullets =collision.GetComponent<Bullet>();
        if (p == null && otherBullets==null)
        {
            DestroyObject(gameObject);
        }
    }
}
