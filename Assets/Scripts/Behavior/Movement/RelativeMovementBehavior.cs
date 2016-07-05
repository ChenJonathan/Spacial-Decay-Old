using UnityEngine;
using System.Collections;

public class RelativeMovementBehavior : Enemy.MovementBehavior
{
    private Vector2 start;

    private readonly Vector2 direction;

    public RelativeMovementBehavior(Vector2 direction, float duration) : base(duration)
    {
        this.direction = direction;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        start = enemy.transform.position;
    }

    public override void Update()
    {
        base.Update();

        Debug.Log(enemy.transform.position.ToString());
        enemy.transform.position = Vector2.Lerp(start, start + direction, time / duration);
    }
}
