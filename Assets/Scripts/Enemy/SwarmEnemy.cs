using UnityEngine;
using System.Collections;
using DanmakU;

public class SwarmEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab bulletPrefab;
    private FireBuilder fireData;

    void Start()
    {
        health = maxHealth = 100 * difficulty;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        fireData = new FireBuilder(bulletPrefab, Field);
        fireData.From(transform);
        fireData.Towards(player);
        fireData.WithSpeed(7);

        AddAttackBehavior(new ConstantAttackBehavior(fireData, 2, 3));
        AddMovementBehavior(new FollowPlayerConstantSpeedBehavior(10, 4 * Random.value + 3, 3));
        AddMovementBehavior(new MoveOverTimeBehavior(this.transform.position + new Vector3(Random.value * 3, Random.value * 3), 3));
        LoopBehaviors = true;
    }

    public override void NormalUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
