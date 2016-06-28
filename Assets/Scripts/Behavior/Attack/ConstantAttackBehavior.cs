using UnityEngine;
using System.Collections;
using DanmakU;

public class ConstantAttackBehavior : Enemy.AttackBehavior
{
    private FireBuilder fireData;

    private float fireRate;
    private float fireDelay;
    private float duration;

    public ConstantAttackBehavior(FireBuilder fireData, float fireRate, float duration)
    {
        this.fireData = fireData.Clone();
        this.fireRate = fireRate;
        this.duration = duration;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        fireDelay = 0;
    }

    public override void Update()
    {
        base.Update();

        if (fireDelay <= 0)
        {
            fireDelay = 1 / fireRate;
            fireData.Fire();
        }
        fireDelay -= Time.deltaTime;

        if (time >= duration)
            End();
    }
}
