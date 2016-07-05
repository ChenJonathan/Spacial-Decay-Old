using UnityEngine;
using System.Collections;

public class LinearMovementBehavior : Enemy.MovementBehavior
{
    private Vector2 start;

    private readonly Vector2 target;

    public LinearMovementBehavior(Vector2 dest, float duration) : base(duration)
    {
        target = dest;
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
        enemy.transform.position = Vector2.Lerp(start, target, time / duration);
	}
}
