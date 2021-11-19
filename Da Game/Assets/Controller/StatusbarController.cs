using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusbarController : MonoBehaviour
{
    public Vector3 offset;
    private RectTransform RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform.position=Camera.main.WorldToScreenPoint(PlayerController.instance.transform.position + offset);
    }
}
