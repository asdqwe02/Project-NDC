using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slam : MonoBehaviour
{
    private Vector3 _knockBack;
    private float _damage;
    public void SetUp(float Damage, Vector3 KnockBack)
    {
        _damage = Damage;
        _knockBack = KnockBack;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy Monster = collision.GetComponent<Enemy>();
        PlayerController p = collision.GetComponent<PlayerController>();
        if (Monster != null)
        {
            return;
        }
        if (p != null)
        {
            Debug.Log("Hit Player");
            p.takeDamage(1, _knockBack);

        }
    }
}
