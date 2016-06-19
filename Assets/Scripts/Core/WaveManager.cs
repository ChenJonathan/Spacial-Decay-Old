using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DanmakU;
using System;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField]
    private List<string> waveList;
    public List<string> Strings
    {
        get
        {
            return waveList;
        }
    }

    private List<Type> typeList;
    public List<Type> Types
    {
        get
        {
            return typeList;
        }
    }

    private Dictionary<string, Type> waveMap;

    public override void Awake()
    {
        base.Awake();

        typeList = new List<Type>();
        waveMap = new Dictionary<string, Type>();
        foreach (string wave in waveList)
        {
            Type waveType = Type.GetType(wave);
            if(waveType != null)
            {
                typeList.Add(waveType);
                waveMap.Add(wave, waveType);
            }
        }
    }

    public Type Get(string wave)
    {
        return waveMap[wave];
    }
}
