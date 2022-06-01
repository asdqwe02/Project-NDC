using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginWaves : MonoBehaviour
{
    private GameObject door; 
    private void Awake() {
        GetComponent<SpriteRenderer>().enabled = false;
        if (door==null)
        {
            door = GameObject.Find("Grid").transform.Find("Door").gameObject;

        }
    }
    public bool start = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !start)
        {
            start = true;
            StartCoroutine(CloseDoor(0.5f));
            PlayerController.instance.transform.position = transform.position; // tp player into the small room
        }
    }
    public void OpenDoor()
    {
        door.SetActive(false);
    }
    IEnumerator CloseDoor(float delay)
    {
        yield return new WaitForSeconds(delay);
        door.SetActive(true);

    }
}
