﻿using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public class TestBehaviorEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab straightBulletPrefab;
    [SerializeField]
    private DanmakuPrefab circleBulletPrefab;

    void Start()
    {
        health = maxHealth = 100 * difficulty;
        
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        FireBuilder straightFireData = new FireBuilder(straightBulletPrefab, Field);
        straightFireData.From(transform);
        straightFireData.Towards(player);
        straightFireData.WithSpeed(5);

        FireBuilder circleFireData = new FireBuilder(circleBulletPrefab, Field);
        circleFireData.From(transform);
        circleFireData.Towards(player);
        circleFireData.WithSpeed(32);
        circleFireData.WithRotation(-24, 24);

        List<AttackBehavior> attacks = new List<AttackBehavior>();
        AddMovementBehavior(new FollowPlayerConstantSpeedBehavior(5, 5, 3));
        AddMovementBehavior(new IdleMovementBehavior(3));
        AddAttackBehavior(new CircularAttackBehavior(circleFireData, 16, 64, 6));
        LoopBehaviors = true;
    }
    
    public override void NormalUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
