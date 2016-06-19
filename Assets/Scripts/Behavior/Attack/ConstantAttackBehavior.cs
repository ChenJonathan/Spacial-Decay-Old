﻿using UnityEngine;
using System.Collections;
using DanmakU;

public class ConstantAttackBehavior : Enemy.AttackBehavior
{
    private FireBuilder fireData;

    float fireRate;
    float fireDelay;
    float duration;

    public ConstantAttackBehavior(FireBuilder fireData, float fireRate, float duration)
    {
        this.fireData = fireData;
        this.fireRate = fireRate;
        this.duration = duration;
    }

    public override void Update()
    {
        base.Update();

        fireDelay -= Time.deltaTime;

        if (fireDelay <= 0)
        {
            fireDelay += 1 / fireRate;
            fireData.Fire();
        }

        if (time >= duration)
            End();
    }
}