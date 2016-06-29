using UnityEngine;
using System.Collections;
using DanmakU;

public class IdleAttackBehavior : Enemy.AttackBehavior
{
    private float duration;

    public IdleAttackBehavior(float duration)
    {
        this.duration = duration;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
    }

    public override void Update()
    {
        base.Update();

        if (time >= duration)
            End();
    }
}
