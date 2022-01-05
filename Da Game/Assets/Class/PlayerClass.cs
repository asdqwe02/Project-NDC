using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerClass : MovingObjects
{
    public static PlayerClass instance;

    [Header("Player Stats")]
    //Variables used in shooting projectiles
    private float baseDamage = 0; //this is use to calculate damage increase or decrease when switching fire mode
    private float maxHP = 0;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _firingTime = 0f;
    [SerializeField] private int _bulletAmount = 3;
    [SerializeField] private int _fireType = 0;
    [SerializeField] private float _meleeAttackRange = 0.5f;
    //Variables used in movement
    [SerializeField] private float _dashRange;

    [Header("Prefab and Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private List<Transform> _bulletPrefab;
    [SerializeField] private Transform _dashPrefab;
    [SerializeField] private Transform _meleeSlashPrefab;

    [Header("Utilities")]
    //Utilities
    public string scenePassword;
    public int coins = 0;
    public int FireType { get => _fireType; set => _fireType = value; }
    public float FiringTime { get => _firingTime; set => _firingTime = value; }
    public float FireRate { get => _fireRate; set => _fireRate = value; }
    public int BulletAmount { get => _bulletAmount; set => _bulletAmount = value; }
    public List<Transform> BulletPrefab { get => _bulletPrefab; set => _bulletPrefab = value; }

    public float DashRange { get => _dashRange; set => _dashRange = value; }
    public Transform DashPrefab { get => _dashPrefab; set => _dashPrefab = value; }
    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public string Buff { get => _buff; set => _buff = value; }
    public Transform MeleeSlashPrefab { get => _meleeSlashPrefab; set => _meleeSlashPrefab = value; }
    public float MeleeAttackRange { get => _meleeAttackRange; set => _meleeAttackRange = value; }
    public float BaseDamage { get => baseDamage; set => baseDamage = value; }
    public float MaxHP { get => maxHP; set => maxHP = value; }
}
