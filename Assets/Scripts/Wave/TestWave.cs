using UnityEngine;
using System.Collections;
using System;

public class TestWave : Wave
{
    private float time;
    private int spawned;

    void Start()
    {
        time = 0;
        spawned = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time >= 4)
        {
            if(spawned == 5)
            {
                End();
            }
            else
            {
                SpawnEnemy("TestEnemy", new Vector2(25, 15));
                time = 0;
                spawned++;
            }
        }
    }
}
