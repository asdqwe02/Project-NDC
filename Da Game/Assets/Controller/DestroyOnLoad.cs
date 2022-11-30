using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLoad : MonoBehaviour
{
    [SerializeField] bool destroyOnLoad;
    private void Awake()
    {
        if (!destroyOnLoad)
            DontDestroyOnLoad(this);

    }
}
