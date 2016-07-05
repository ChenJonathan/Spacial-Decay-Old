using UnityEngine;
using System.Collections;
using System;
using DanmakU;
using DanmakU.Modifiers;
using DanmakU.Controllers;

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
            .AddAttackBehavior(new ConstantAttackBehavior(shurikenPrefab, 12, 8, Color.red, 1))
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

        SpawnEnemy(ninjaEnemy1, new Vector2(-25, 8));
        SpawnEnemy(ninjaEnemy2, new Vector2(25, 8));

    }
    
    public override void NormalUpdate()
    {
        if ((timeElapsed += Time.deltaTime) >= 6)
        {
            End();
        }
    }


}
