using UnityEngine;
using System.Collections;

public class IdleMovementBehavior : Enemy.MovementBehavior
{
    private float totalTime;

    public IdleMovementBehavior(float interval)
    {
        totalTime = interval;
    }

    public override void Update()
    {
        base.Update();

        if (time >= totalTime)
            End();
    }
}
