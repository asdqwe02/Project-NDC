using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpdated : MonoBehaviour
{
    [SerializeField] private float distanceLaser = 100;
    [SerializeField] private LayerMask laserLayerMask;
    [SerializeField] private float LaserDelayTime = 0.75f, LaserDelayCountDown;
    private bool EndOfLaserDelay = false;

    public LineRenderer m_linerenderer;
    Transform m_transform;
    private bool rotating = true;
    private bool rotating2 = false;
    float DelayTime = 0.75f;
    public bool endOfLaser = false;
    public bool start = false;

    private bool RotationFix = true;


    void Start()
    {
        LaserDelayCountDown = LaserDelayTime;
        m_transform = GetComponent<Transform>();
    }
    void Update()
    {
        ShootLaser();
    }
    void ShootLaser()
    {

        if (start && EndOfLaserDelay)
        {
            if (RotationFix)
            {
                RotationFix = false;
                Vector3 temp = transform.rotation.eulerAngles;
                temp.y = 0;
                transform.rotation = Quaternion.Euler(temp);
            }
            rotate();
            RaycastHit2D _hit = Physics2D.Raycast(m_transform.position, m_transform.right, distanceLaser, laserLayerMask);
            // Debug.Log(_hit.transform.tag);
            if (_hit)
            {
                Draw2DRay(m_transform.position, _hit.point);
                if (_hit.transform.CompareTag("Player"))
                {
                    float LaserDamage = PlayerController.instance.MaxHP*0.2f;
                    PlayerController.instance.takeDamage(LaserDamage, MovingObjects.DamageType.Cold);
                }
            }
            else
            {
                Draw2DRay(m_transform.position, m_transform.right * distanceLaser);
            }
        }
        else
        {
            Draw2DRay(m_transform.position, m_transform.position);
            rotating = true;
            rotating2 = false;
            DelayTime = 0.75f;
        }
    }

    void rotate()
    {
        if (rotating)
        {

            Vector3 to = new Vector3(0, 0, 179);
            if (Vector3.Distance(to, transform.rotation.eulerAngles) >= 0.5)
            {
                transform.Rotate(0, 0, (Time.deltaTime + 0.5f) * -1);
            }
            else
            {
                transform.eulerAngles = to;
                rotating = false;
                rotating2 = true;
            }

        }
        if (!rotating && rotating2)
        {
            DelayTime -= Time.deltaTime;
            if (DelayTime <= 0)
            {
                Vector3 to = new Vector3(0, 0, 0);
                if (Vector3.Distance(to, transform.rotation.eulerAngles) >= 0.5)
                {
                    transform.Rotate(0, 0, (Time.deltaTime + 0.5f) * 1);
                }
                else
                {
                    transform.eulerAngles = to;
                    rotating2 = false;
                    endOfLaser = true;
                    RotationFix = true;
                    EndOfLaserDelay = false;
                }
            }

        }
    }
    void Draw2DRay(Vector2 StartPos, Vector2 endPos)
    {
        m_linerenderer.SetPosition(0, StartPos);
        m_linerenderer.SetPosition(1, endPos);

    }
    public void StartLaser()
    {
        if(!start)  
            InvokeRepeating("StartLaserAfterDelay", 0f, Time.fixedDeltaTime);
        start = true;
    }
    private void StartLaserAfterDelay()
    {
        LaserDelayCountDown -= Time.deltaTime;
        if (LaserDelayCountDown<=0)
        {
            EndOfLaserDelay = true;
            LaserDelayCountDown = LaserDelayTime;
            CancelInvoke("StartLaserAfterDelay");
        }    
    }
}
