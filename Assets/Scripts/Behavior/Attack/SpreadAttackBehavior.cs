using UnityEngine;
using System.Collections;
using DanmakU;

public class SpreadAttackBehavior : Enemy.AttackBehavior
{
    private FireBuilder fireData;

    private float fireRate;
    private float fireDelay;
    private int bullets;
    private float spread;
    private float angle;
    private float duration;

    public SpreadAttackBehavior(FireBuilder fireData, float fireRate, int bullets, float spread, float duration)
    {
        this.fireData = fireData.Clone();
        this.fireRate = fireRate;
        this.bullets = bullets;
        this.spread = spread;
        angle = spread / bullets;
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
            for (float a = -spread / 2; a < spread / 2; a += angle)
            {
                fireData.WithRotation(a);
                fireData.Fire();
            }
        }
        fireDelay -= Time.deltaTime;

        if (time >= duration)
            End();
    }
}
