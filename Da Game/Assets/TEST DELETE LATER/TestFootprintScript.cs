using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFootprintScript : MonoBehaviour
{
    [SerializeField] GameObject footprintPrefab;
    Vector3 originalPos;
    [SerializeField] float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0f;
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (originalPos != transform.position)
        {
            timeElapsed += Time.deltaTime;
            originalPos = transform.position;
        }
        if (timeElapsed > 0.2f)
        {
            Instantiate(footprintPrefab, transform.position, transform.rotation);
            timeElapsed = 0f;
        }
    }
}
