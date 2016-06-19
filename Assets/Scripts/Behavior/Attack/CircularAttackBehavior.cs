using UnityEngine;
using System.Collections;
using DanmakU;

public class CircularAttackBehavior : Enemy.AttackBehavior
{
    private FireBuilder fireData;

    private float fireRate;
    private float fireDelay;
    private float angleOffset;
    private float duration;

    public CircularAttackBehavior(FireBuilder fireData, float fireRate, int perWave, float duration)
    {
        this.fireData = fireData.Clone();
        this.fireRate = fireRate;
        angleOffset = 360f / perWave;
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

        fireDelay -= Time.deltaTime;

        if (fireDelay <= 0)
        {
            fireDelay += 1 / fireRate;
            for (float angle = 0; angle < 360; angle += angleOffset)
            {
                fireData.WithRotation(angle);
                fireData.Fire();
            }
        }

        if (time >= duration)
            End();
    }
}
