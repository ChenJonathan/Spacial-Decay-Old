using UnityEngine;
using System.Collections;

public class MoveOverTimeBehavior : Enemy.MovementBehavior
{
    private Vector2 start;
    private Vector2 target;
    private float totalTime;

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

        Debug.Log(enemy.transform.position.ToString());
        enemy.transform.position = Vector2.Lerp(start, target, time / totalTime);
        if (time >= totalTime)
            End();
	}
}
