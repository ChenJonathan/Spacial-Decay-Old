using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DanmakU;
using System;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField]
    private List<Wave> waveList;
    public List<Wave> List
    {
        get
        {
            return waveList;
        }
    }

    public Wave Get(int wave)
    {
        return waveList[wave];
    }

    public Wave GetRandom()
    {
        return waveList[UnityEngine.Random.Range(0, waveList.Count)];
    }
}
