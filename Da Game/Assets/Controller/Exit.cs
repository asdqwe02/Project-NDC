using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    public string sceneName;
    [SerializeField] private string newScenePassword;
    [SerializeField] private string exitTagCondition;
    GameObject[] monstersAlive;
    Buff buff;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(exitTagCondition))
        {
            monstersAlive = GameObject.FindGameObjectsWithTag(exitTagCondition);
            buff = GameObject.FindObjectOfType<Buff>();
            if (monstersAlive.Length != 0 || buff!=null)
                return;
        }
        if(other.tag =="Player")
        {
            PlayerController.instance.scenePassword = newScenePassword;
            SceneManager.LoadScene(sceneName);
        }
    }
}
