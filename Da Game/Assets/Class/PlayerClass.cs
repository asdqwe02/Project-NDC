using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerClass : MovingObjects
{
    public static PlayerClass instance;
    [SerializeField] private Rigidbody2D rb;
    //Variables used in Shooting 

    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _firingTime = 0f;
    [SerializeField] private int _bulletAmount = 3;
    [SerializeField] private int _fireType = 0;
    [SerializeField] private Transform _bulletPrefab;

    //Variables used in movement
   
    [SerializeField] private float _dashRange;
    [SerializeField] private Transform _dashPrefab;

    //Utilities
    public string scenePassword;
    [SerializeField] private string _buff = "";

    public int FireType { get => _fireType; set => _fireType = value; }
    public float FiringTime { get => _firingTime; set => _firingTime = value; }
    public float FireRate { get => _fireRate; set => _fireRate = value; }
    public int BulletAmount { get => _bulletAmount; set => _bulletAmount = value; }
    public Transform BulletPrefab { get => _bulletPrefab; set => _bulletPrefab = value; }

    public float DashRange { get => _dashRange; set => _dashRange = value; }
    public Transform DashPrefab { get => _dashPrefab; set => _dashPrefab = value; }
    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public string Buff { get => _buff; set => _buff = value; }

    void Dash()
    {
    }
    void ChargedAttack()
    {

    }
    void Interact()
    {

    }

}
