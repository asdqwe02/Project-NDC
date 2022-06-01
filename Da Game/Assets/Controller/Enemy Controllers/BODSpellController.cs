using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODSpellController : MonoBehaviour
{
  
    private float _damage=1;
    MovingObjects.DamageType _damageType = MovingObjects.DamageType.Lightning;
    private void Awake() {
        
    }
    public void SetUp(float damage, MovingObjects.DamageType damageType)
    {
        _damage = damage;
        _damageType = damageType;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Bringer of Death spell hit player");
            float x_dir = PlayerController.instance.transform.position.x - transform.position.x ;
            float knockBackX_Dir = 0.25f; // look stupid
            if (x_dir<0)
                knockBackX_Dir = -knockBackX_Dir;
            Vector2 knockBack = new Vector2(knockBackX_Dir,0);
            PlayerController.instance.takeDamage(_damage,_damageType,knockBack);
        }
    }
}
