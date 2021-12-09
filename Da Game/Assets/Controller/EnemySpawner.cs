using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING, FINISHED };
    SpriteRenderer sr;
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public BeginWaves begin;
    private int nextWave = 0;

    public float timeBetweenWave = 2.5f;
    public float waveCountdown;

    public float searchCountdown = 5;
    private SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountdown = timeBetweenWave;
        sr = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        if (begin.start)
        {
            if (!sr.enabled)
                sr.enabled=true;
            if (state == SpawnState.FINISHED)
                return;
            if (state == SpawnState.WAITING)
            {

                if (!EnemyIsAlive())
                {
                    WaveComplete();
                }
                else
                {
                    return;
                }

            }
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;      
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            { 
                return false;
            }
        }
        return true;
    }


    void WaveComplete()
    {
        //??? explain ???
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWave;
        if (nextWave + 1 > waves.Length - 1)
        {
            state = SpawnState.FINISHED;
        }
        else
        {
            nextWave++;
        }
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("spawning wave +" + _wave.name);
        state = SpawnState.SPAWNING;

        //Spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        
        yield break;
    }
    public SpawnState GetSpawnState()
    {
        return state;
    }
    void SpawnEnemy(Transform _enemy)
    {
        //spawn enemies
        //Debug.Log("spawning enemy: " + _enemy.name);
        Instantiate(_enemy, transform.position, transform.rotation);

    }
}
