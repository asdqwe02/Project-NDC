using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform buff;
    public Buff.BuffType buffType; //TODO: change this to buff pool 
    PlayerController pc;
    GameObject[] Spawner, StartPlate, MonsterAlive;
    bool[] conditions;
    bool spawned = false;
    void Start()
    {
        pc = PlayerController.instance;
        buff.GetComponent<Buff>().SetUp(buffType);
        conditions = new bool[3] { false, false, false };
        Spawner = GameObject.FindGameObjectsWithTag("EnemySpawner");
        StartPlate = GameObject.FindGameObjectsWithTag("StartPlate");

    }


    private void SpawnBuff()
    {
        if (!spawned)
        {
            Vector3 spawnPosition = pc.transform.position;
            spawnPosition.x += Random.Range(-0.2f, 0.2f);
            spawnPosition.y += Random.Range(-0.2f, 0.2f);
            Instantiate(buff, spawnPosition, Quaternion.identity);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!conditions[0])
        {
            int c = 0;
            foreach (GameObject startplate in StartPlate)
            {
                if (startplate.GetComponent<BeginWaves>().start) //doing this might be inefficient
                    c++;
            }
            if (c == StartPlate.Length)
                conditions[0] = true;
        }
        if (!conditions[1])
        {
            int c = 0;
            foreach (GameObject spawner in Spawner)
            {
                if (spawner.GetComponent<EnemySpawner>().GetSpawnState() == EnemySpawner.SpawnState.FINISHED)//doing this might be inefficient
                    c++;
            }
            if (c == Spawner.Length)
                conditions[1] = true;
        }
        if (!conditions[2])
        {
            MonsterAlive = GameObject.FindGameObjectsWithTag("Enemy");
            if (MonsterAlive.Length == 0)
                conditions[2] = true;
        }
        if (conditions[0] && conditions[1] && conditions[2])
        {
            SpawnBuff();
            spawned = true;
        }
    }
}