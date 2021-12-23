using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusbarController : MonoBehaviour
{
    public Vector3 offset;
    public RectTransform CircularHealthbar;
    private RectTransform RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform.position = CircularHealthbar.transform.position + offset;
    }
}
