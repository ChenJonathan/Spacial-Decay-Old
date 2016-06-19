using UnityEngine;
using System.Collections;

public class MoveOverTimeBehavior : Enemy.MovementBehavior
{
    Vector2 start;
    Vector2 target;
    float totalTime;

    public MoveOverTimeBehavior(Vector2 dest, float interval)
    {
        target = dest;
        totalTime = interval;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        start = enemy.transform.position;
    }

    public override void Update ()
    {
        base.Update();
        enemy.transform.position = Vector2.Lerp(start, target, time / totalTime);
        if (time >= totalTime)
            End();
	}
}
