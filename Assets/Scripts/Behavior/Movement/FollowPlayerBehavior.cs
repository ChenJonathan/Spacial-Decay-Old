using UnityEngine;
using System.Collections;

public class FollowPlayerBehavior : Enemy.MovementBehavior
{
    private float speed;
    private float deltaSpeed;
    private float distanceFromPlayer;
    private float distanceToMove;

    private readonly float initialSpeed;

    public FollowPlayerBehavior(float speed, float deltaSpeed, float distanceFromPlayer, float duration) : base(duration)
    {
        this.speed = initialSpeed = speed;
        this.deltaSpeed = deltaSpeed;
        this.distanceFromPlayer = distanceFromPlayer;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        
        speed = initialSpeed;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
        Vector2 destination = (enemy.transform.position - player.transform.position) / distance * distanceFromPlayer + player.transform.position;
        if (enemy.transform.position != player.transform.position)
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, destination, speed * Time.fixedDeltaTime);
        speed += deltaSpeed * Time.fixedDeltaTime;
	}
}
