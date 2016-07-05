﻿using UnityEngine;
using System.Collections;
using System;
using DanmakU;
using DanmakU.Modifiers;
using DanmakU.Controllers;

public class TestWave : Wave
{
    private int killed;

    [SerializeField]
    private Enemy swarmPrefab;
    [SerializeField]
    private DanmakuPrefab bulletPrefab;
    [SerializeField]
    private DanmakuPrefab circlePrefab;

    public void Start()
    {
        SpawnData swarmEnemy = new SpawnData(swarmPrefab, 100 * difficulty)
            .AddAttackBehavior(new IdleAttackBehavior(3))
            .AddAttackBehavior(new ConstantAttackBehavior(bulletPrefab, 8, 12, Color.cyan, 1))
            .AddAttackBehavior(new IdleAttackBehavior(3))
            .AddAttackBehavior(new CircleAttackBehavior(circlePrefab, 4, 1, 330, 12, 0, 0, Color.red, 1))
            .AddMovementBehavior(new FollowPlayerBehavior(8, -2, 2, 3))
            .AddMovementBehavior(new IdleMovementBehavior(3));
        swarmEnemy.FacePlayer = true;
        swarmEnemy.LoopBehaviors = true;

        SpawnEnemyChain(swarmEnemy, 4, new Vector2(25, 15), Vector2.zero, 5);
    }

    public override void OnEnemyDeath(Enemy enemy)
    {
        base.OnEnemyDeath(enemy);
        
        killed++;
        if (killed == 5)
            End();
    }
}
