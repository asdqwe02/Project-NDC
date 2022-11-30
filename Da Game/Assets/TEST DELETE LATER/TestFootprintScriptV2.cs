using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFootprintScriptV2 : MonoBehaviour
{
    public float footprintSpawnTime;
    public float timeElapsed;
    public bool flip = false;
    [SerializeField] Vector3 originalPos;
    public GameObject footprintPrefab;

    private void Start()
    {
        timeElapsed = 0;
        originalPos = transform.position;
    }

    void Update()
    {
        if (originalPos != transform.position)
        {
            timeElapsed += Time.deltaTime;
            originalPos = transform.position;
        }
        if (timeElapsed >= footprintSpawnTime)
        {
            GameObject footprint = Instantiate(footprintPrefab, transform.position, transform.rotation);
            if (flip)
            {
                Vector3 scale = footprint.transform.localScale;
                scale.x *= -1;
                footprint.transform.localScale = scale;
            }
            flip = !flip;
            timeElapsed = 0f;
        }
    }
}
