using UnityEngine;
using System.Collections;
using System;
using DanmakU;
using DanmakU.Modifiers;
using DanmakU.Controllers;
using System.Collections.Generic;

public class ShurikenWave : Wave
{
    private float timeElapsed;

    [SerializeField]
    private Enemy ninjaPrefab;
    [SerializeField]
    private DanmakuPrefab shurikenPrefab;

    public void Start()
    {
        SpawnData ninjaEnemy1 = new SpawnData(ninjaPrefab, 100 * difficulty)
            .AddAttackBehavior(new IdleAttackBehavior(1))
            .AddAttackBehavior(new CircleAttackBehavior(shurikenPrefab, 12, 8, new DynamicInt(8, 16), 1).AddController(new RotateController(2)))
            .AddAttackBehavior(new IdleAttackBehavior(1))
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(35, 0), 1))
            .AddMovementBehavior(new IdleMovementBehavior(1))
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(-35, 0), 1));
        ninjaEnemy1.FacePlayer = true;
        ninjaEnemy1.LoopBehaviors = false;

        SpawnData ninjaEnemy2 = ((SpawnData)ninjaEnemy1.Clone())
            .ClearMovementBehavior()
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(-35, 0), 1))
            .AddMovementBehavior(new IdleMovementBehavior(1))
            .AddMovementBehavior(new RelativeMovementBehavior(new Vector2(35, 0), 1));

        List<Vector2> randLoc1 = new List<Vector2>();
        List<Vector2> randLoc2 = new List<Vector2>();

        for (int i = 0; i < 4; i++)
        {
            randLoc1.Add(new Vector2(-25, new DynamicFloat(-10, 10)));
            randLoc2.Add(new Vector2(25, new DynamicFloat(-10, 10)));
        }

        SpawnEnemyChain(ninjaEnemy1, 4, randLoc1);
        SpawnEnemyChain(ninjaEnemy2, 4, randLoc2);
    }
    
    public override void NormalUpdate()
    {
        if ((timeElapsed += Time.deltaTime) >= 17)
        {
            End();
        }
    }


}
