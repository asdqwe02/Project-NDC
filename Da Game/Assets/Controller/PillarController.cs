using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    private Animator animator;

    private BoxCollider2D collider;
    [SerializeField] private Transform _explosionPrefab;
    [SerializeField] private Transform ExplosionPoint;
    public bool Active=true;
    private float _damage;
    public float LiveTime = 5;
    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(animator.GetBool("IsStanding"))
        {
            LiveTime -= Time.deltaTime;
            if(LiveTime <= 0 )
            {
                animator.SetBool("IsCrumbling", true);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Active)
        {
            Enemy Monster = collision.GetComponent<Enemy>();
            PlayerController p = collision.GetComponent<PlayerController>();
            if (Monster != null)
            {
                return;
            }
            if (p != null)
            {
                p.takeDamage(1, MovingObjects.DamageType.Fire,new Vector3(0,0,0));
            }
        }
    }
    private void NotInflicting()
    {
        Active = false;
        animator.SetBool("IsStanding", true);
    }

    private void End()
    {
        Vector3 scale = new Vector3(0.6f,2f,0f);
        Transform Explosion = Instantiate(_explosionPrefab, ExplosionPoint.position, Quaternion.identity);
        Explosion.GetComponent<ExplosionController>().SetUp(10,scale);
        Destroy(gameObject);

    }
}
