using UnityEngine;
using System.Collections;
using System;

public class SimulSpawnWave : Wave
{
    private int killed;

    public void Start()
    {
        SpawnEnemyChain("TestModifierEnemy", 0, new Vector2(25, 15), new Vector2(0, -7.5f), 5);
    }

    public override void OnEnemyDeath(Enemy enemy)
    {
        base.OnEnemyDeath(enemy);

        killed++;
        if (killed == 5)
            End();
    }
}
