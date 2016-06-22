using UnityEngine;
using System.Collections;

public class FollowPlayerConstantSpeedBehavior : Enemy.MovementBehavior
{
    private float speed;
    private float distanceFromPlayer;
    private float distanceToMove;
    private float totalTime;

    public FollowPlayerConstantSpeedBehavior(float speed, float distanceFromPlayer, float totalTime)
    {
        this.speed = speed;
        this.distanceFromPlayer = distanceFromPlayer;
        this.totalTime = totalTime;
    }

    public override void Update ()
    {
        base.Update();

        float distance = Vector2.Distance(player.transform.position, enemy.transform.position);

        Vector2 destination = (enemy.transform.position - player.transform.position) / distance * distanceFromPlayer + player.transform.position;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, destination, speed * Time.deltaTime);
        if (time >= totalTime)
            End();
	}
}
