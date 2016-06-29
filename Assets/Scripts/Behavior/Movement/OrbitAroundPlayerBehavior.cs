using UnityEngine;
using System.Collections;
using DanmakU;

public class OrbitAroundPlayerBehavior : Enemy.MovementBehavior
{
    private float radialSpeed;
    private float angularSpeed;
    private float radius;
    private float totalTime;

    private Vector3 previousPlayerPosition;
    private Vector3 playerDisplacement;

    public OrbitAroundPlayerBehavior(float radialSpeed, float angularSpeed, DynamicFloat radius, float totalTime)
    {
        this.radialSpeed = radialSpeed;
        this.angularSpeed = angularSpeed;
        this.radius = radius;
        this.totalTime = totalTime;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        previousPlayerPosition = player.transform.position;
        playerDisplacement = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();

        float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
        Vector2 destination = (enemy.transform.position - player.transform.position) * radius / distance + player.transform.position;

        if (Mathf.Abs(distance - radius) > 0.01)
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, destination, radialSpeed * Time.deltaTime);
        enemy.transform.RotateAround(player.transform.position, Vector3.back, radius * angularSpeed * Time.deltaTime);

        Vector3 newTarget = Vector2.MoveTowards(enemy.transform.position, enemy.transform.position + playerDisplacement, 10 * Time.deltaTime);
        Vector3 enemyDisplacement = newTarget - enemy.transform.position;
        enemy.transform.position = newTarget;
        playerDisplacement += player.transform.position - previousPlayerPosition - enemyDisplacement;
        previousPlayerPosition = player.transform.position;

        if (time >= totalTime)
            End();
    }
}
