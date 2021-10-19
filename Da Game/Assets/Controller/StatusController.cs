using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    public RectTransform m_RectTransform;
    public Vector3 offset;
    public Slider slider;
    PlayerController Player;
    private void Start()
    {
        Player = GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {

        m_RectTransform.transform.position = slider.transform.position + offset;

    }
}

