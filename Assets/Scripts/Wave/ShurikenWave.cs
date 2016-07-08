using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Modifiers;
using DanmakU.Controllers;
using System.Collections.Generic;

public class ShurikenWave : Wave
{
    private float previousTimeElapsed;
    private float timeElapsed;

    [SerializeField]
    private Enemy ninjaPrefab;
    [SerializeField]
    private DanmakuPrefab shurikenPrefab;
    [SerializeField]
    private DanmakuPrefab circlePrefab;

    private SpawnData ninjaEnemy1;
    private SpawnData ninjaEnemy2;
    private SpawnData ninjaEnemy3;

    public void Start()
    {
        ninjaEnemy1 = new SpawnData(ninjaPrefab, 100 * difficulty)
            .AddAttackBehavior(new IdleAttackBehavior(1))
            .AddAttackBehavior(new CircleAttackBehavior(circlePrefab, 12, 8, new DynamicInt(8, 12), 1))
            .AddAttackBehavior(new IdleAttackBehavior(1))
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(35, 0), 1))
            .AddMovementBehavior(new IdleMovementBehavior(1))
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(-35, 0), 1));
        ninjaEnemy1.FacePlayer = true;
        ninjaEnemy1.LoopBehaviors = false;

        ninjaEnemy2 = ((SpawnData)ninjaEnemy1.Clone())
            .ClearMovementBehavior()
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(-35, 0), 1))
            .AddMovementBehavior(new IdleMovementBehavior(1))
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(35, 0), 1));

        ninjaEnemy3 = new SpawnData(ninjaPrefab, 100 * difficulty)
            .AddAttackBehavior(new IdleAttackBehavior(1))
            .AddAttackBehavior(new ConstantAttackBehavior(shurikenPrefab, false, 12, 8, 1).AddController(new RotateController(2)))
            .AddAttackBehavior(new IdleAttackBehavior(.5f))
            .AddMovementBehavior(new IdleMovementBehavior(3.5f));
        ninjaEnemy3.FacePlayer = true;
        ninjaEnemy3.LoopBehaviors = false;
    }
    
    public override void NormalUpdate()
    {
        previousTimeElapsed = timeElapsed;
        if ((timeElapsed += Time.deltaTime) >= 22)
        {
            End();
        }
        
        if (TimeElapsed(.1f))
        {
            List<Vector2> randLoc1 = new List<Vector2>();
            List<Vector2> randLoc2 = new List<Vector2>();

            for (int i = 0; i < 2; i++)
            {
                randLoc1.Add(new Vector2(-25, new DynamicInt(-8, 8)));
                randLoc2.Add(new Vector2(25, new DynamicInt(-8, 8)));
            }

            SpawnEnemyChain(ninjaEnemy1, 10, randLoc1);
            SpawnEnemyChain(ninjaEnemy2, 10, randLoc2);
        }
        else if (TimeElapsed(0, 0.5f) && ((int)timeElapsed % 10 >= 3))
        {
            Vector2 spawn = Random.insideUnitCircle.normalized * 25;
            SpawnData ninjaEnemy = ((SpawnData)ninjaEnemy3.Clone())
                .ClearMovementBehavior()
                .AddMovementBehavior(new LinearMovementBehavior((Vector2)player.transform.position + Random.insideUnitCircle.normalized * 8, 1))
                .AddMovementBehavior(new IdleMovementBehavior(1))
                .AddMovementBehavior(new LinearMovementBehavior(spawn, .5f));
            SpawnEnemy(ninjaEnemy, spawn);
        }
    }

    private bool TimeElapsed(float time)
    {
        return time > previousTimeElapsed && time <= timeElapsed;
    }
    
    private bool TimeElapsed(float offset, float interval)
    {
        float time = offset;
        while (time < previousTimeElapsed)
        {
            time += interval;
        }
        return TimeElapsed(time);
    }
}
