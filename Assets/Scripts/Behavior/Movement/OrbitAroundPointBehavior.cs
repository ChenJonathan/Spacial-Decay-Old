using UnityEngine;
using System.Collections;
using DanmakU;

public class OrbitAroundPointBehavior : Enemy.MovementBehavior
{
    private float radialSpeed;
    private float angularSpeed;
    private float radius;
    private Vector3 point;

    public OrbitAroundPointBehavior(float radialSpeed, float angularSpeed, DynamicFloat radius, Vector3 point, float duration) : base(duration)
    {
        this.radialSpeed = radialSpeed;
        this.angularSpeed = angularSpeed;
        this.radius = radius;
        this.point = point;
    }
    
    public override void Update()
    {
        base.Update();

        float distance = Vector2.Distance(point, enemy.transform.position);
        Vector2 destination = (enemy.transform.position - point) * radius / distance + point;

        if (Mathf.Abs(distance - radius) > 0.01)
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, destination, radialSpeed * Time.deltaTime);
        enemy.transform.RotateAround(point, Vector3.back, radius * angularSpeed * Time.deltaTime);
    }
}
