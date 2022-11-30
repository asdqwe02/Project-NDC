using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    

    private MovingObjects.DamageType _damageType = MovingObjects.DamageType.Fire;
    [SerializeField]
    private float _damage=0;
    void Start()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ExplosionFire1,transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Vector2 knockBack = (PlayerController.instance.transform.position - transform.position ).normalized;
        
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().takeDamage(_damage,_damageType,knockBack);
        }
    }
    public void SetUp(float damage, Vector3 scale)
    {   
        _damage=damage;
        transform.localScale = scale;
    }
}
