using UnityEngine;
using System.Collections;

public class OrbitPlayerConstantSpeedBehavior : Enemy.MovementBehavior
{
    private float speed;
    private float angleToMove;
    private float radius;
    private float distance;
    private float totalTime;
    private Quaternion angle;

    public OrbitPlayerConstantSpeedBehavior(float speed, float radius, float totalTime)
    {
        this.speed = speed;
        this.radius = radius;
        this.totalTime = totalTime;
    }

    public override void Update()
    {
        base.Update();

        distance = Vector2.Distance(player.transform.position, enemy.transform.position);

        Vector2 destination = (enemy.transform.position - player.transform.position) * radius / distance + player.transform.position;
        if (Mathf.Abs(distance - radius) >= 0.1f)
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, destination, speed * Time.deltaTime);
        }
        else
        {
            destination += new Vector2(-destination.y, destination.x);
            enemy.transform.RotateAround(player.transform.position, Vector3.forward, speed * radius * Time.deltaTime);
        }

        if (time >= totalTime)
            End();
    }
}
