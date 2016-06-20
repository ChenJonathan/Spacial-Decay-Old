using UnityEngine;
using System.Collections;
using System;

public class SimulSpawnWave : Wave
{
    private int spawned;
    private int killed;

    void Start()
    {
        spawned = 0;
    }

    public override void NormalUpdate()
    {
        if (spawned < 5)
        {
            SpawnEnemy("TestBehaviorEnemy", new Vector2(25 - spawned * 5, 15));
            spawned++;
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
