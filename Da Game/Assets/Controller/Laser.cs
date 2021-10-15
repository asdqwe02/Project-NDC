using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float distanceLaser = 100;
    public Transform laserFirePoint;
    public LineRenderer m_linerenderer;
    Transform m_transform;
    private bool rotating = true;
    private bool rotating2 = false;
    float DelayTime = 1;
    public bool endOfLaser = false;
    public bool start = false;
    void Start()
    {
        m_transform = GetComponent<Transform>();
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaser();
    }
    void ShootLaser()
    {

        if (start)
        {
            if (rotating)
            {
                Vector3 to = new Vector3(0, 0, 180);
                if (Vector3.Distance(transform.eulerAngles, to) > 1)
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
                    if (Vector3.Distance(transform.eulerAngles, to) > 1)
                    {
                        transform.Rotate(0, 0, (Time.deltaTime + 0.5f));
                    }
                    else
                    {
                        transform.eulerAngles = to;
                        rotating2 = false;
                        endOfLaser = true;
                    }
                }
            }


            RaycastHit2D _hit = Physics2D.Raycast(m_transform.position, transform.right);
            if (_hit != null)
            {
                Draw2DRay(laserFirePoint.position, _hit.point);
            }
            else
            {
                Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right * distanceLaser);
            }
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.position);
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
