using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoorController : MonoBehaviour
{
    GameObject door;
    public static DungeonDoorController instance;
    GameObject [] monsterAlive;
    private void Awake() {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        if (door==null)
            door = GameObject.Find("Grid").transform.Find("Door").gameObject;
    }
    public void OpenDoor()
    {
        door.SetActive(false);
    }
    public void CloseDoor()
    {
        door.SetActive(true);
    }
    private void FixedUpdate() {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            OpenDoor();
        else CloseDoor();
    }

}
