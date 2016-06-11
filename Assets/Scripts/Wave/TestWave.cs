using UnityEngine;
using System.Collections;
using System;

public class TestWave : Wave
{
    private float time;
    private int spawned;

    public override void Start()
    {
        time = 0;
        spawned = 0;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        if(time >= 4)
        {
            if(spawned == 1)
            {
                End();
            }
            else
            {
                Spawn("TestEnemy", new Vector2(25, 15));
                time = 0;
                spawned++;
            }
        }
    }
}
