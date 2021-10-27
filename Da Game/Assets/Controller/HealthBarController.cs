using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Vector3 offset;

    private void Start()
    {
        



    }
    private void Update()
    {
        transform.position = PlayerController.instance.transform.position + offset;
    }
}
