using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasController : MonoBehaviour
{
    public static MainCanvasController instance;
    private void Awake() {
        if (instance ==null)
            instance = this;
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

 
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
