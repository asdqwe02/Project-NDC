using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuffSpawnController : MonoBehaviour
{
    public Transform[] buffs; //list or array idk which is better
    public Transform buffPrefab;
    private struct TutorialBuffs
    {
        public Buff.BuffType buffType;
        public Vector3 buffsPosition;
    }
    TutorialBuffs[] TBuffs;
    void Start()
    {
        
        if (buffs.Length > 0)
        {
            TBuffs = new TutorialBuffs[buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                TBuffs[i].buffsPosition = buffs[i].position;
                TBuffs[i].buffType = buffs[i].GetComponent<Buff>().GetBuff();
            }
        }
    }
    private void FixedUpdate()
    {
        CheckBuffConsumedAndRespawn();   
    }
    private void CheckBuffConsumedAndRespawn()
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i] == null)
            {
                buffPrefab.GetComponent<Buff>().SetUp(TBuffs[i].buffType);
                Transform RespawnBuff = Instantiate(buffPrefab, TBuffs[i].buffsPosition, Quaternion.identity);
                buffs[i] = RespawnBuff;
            }
        }
    }
}
