using UnityEngine;
using System.Collections;
using System;

public class TestWave : Wave
{
    private float time;
    private int spawned;
    private int killed;

    void Start()
    {
        time = 0;
        spawned = 0;
    }

    public override void NormalUpdate()
    {
        time += Time.deltaTime;
        if(time >= 4)
        {
            if(spawned < 5)
            {
                SpawnEnemy("TestCrescentEnemy", new Vector2(0, 15));
                time = 0;
                spawned++;
            }
        }
    }

    public override void OnEnemyDeath(Enemy enemy)
    {
        base.OnEnemyDeath(enemy);

        killed++;
        if (killed == 5)
            End();
    }
}
