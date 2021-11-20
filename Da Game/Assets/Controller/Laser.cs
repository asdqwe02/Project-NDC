using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float distanceLaser = 100;
    [SerializeField] private LayerMask laserLayerMask;


    public Quaternion temp;
    public LineRenderer m_linerenderer;
    Transform m_transform;
    private bool rotating = true;
    private bool rotating2 = false;
    float DelayTime = 1;
    public bool endOfLaser = false;
    public bool start = false;

    private bool RotationFix = true;

    void Start()
    {
        //transform.eulerAngles = new Vector3(0, 0, 345);
        m_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaser();
    }
    void Rotate()
    {
        
        if (rotating)
        {
            Vector3 to = new Vector3(0, -180, 224);
            if (Vector3.Distance(transform.eulerAngles, to) > 0.5)
            {
                transform.Rotate(0, 0, (float)(Time.deltaTime *10) * 1);
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

                Vector3 to = new Vector3(0, -180, 125);
                if (Vector3.Distance(transform.eulerAngles, to) > 0.5)
                {
                    transform.Rotate(0, 0, (float)(Time.deltaTime * 10) * -1);
                }
                else
                {
                    transform.eulerAngles = to;
                    rotating2 = false;
                    endOfLaser = true;
                    RotationFix = true;
                }
            }
        }
    }

    void ShootLaser()
    {

        if (start)
        {
            if(RotationFix)
            {
                
                RotationFix = false;
                transform.rotation = temp;

            }
            transform.rotation = temp;
            //Rotate();
            RaycastHit2D _hit = Physics2D.Raycast(m_transform.position, m_transform.right , distanceLaser, laserLayerMask);
            Draw2DRay(m_transform.position, m_transform.right);
            if (_hit != null) 
            {
                Draw2DRay(m_transform.position, _hit.point);
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
            DelayTime = 1;
        }
            
        


    }
    void Draw2DRay(Vector2 StartPos, Vector2 endPos)
    {
        m_linerenderer.SetPosition(0, StartPos);
        m_linerenderer.SetPosition(1, endPos);

    }
}
