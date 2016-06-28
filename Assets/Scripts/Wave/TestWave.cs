using UnityEngine;
using System.Collections;
using System;

public class TestWave : Wave
{
    private int killed;

    public void Start()
    {
        SpawnEnemyChain("TestModifierEnemy", 4, new Vector2(25, 15), Vector2.zero, 5);
    }

    public override void OnEnemyDeath(Enemy enemy)
    {
        base.OnEnemyDeath(enemy);

        killed++;
        if (killed == 5)
            End();
    }
}
