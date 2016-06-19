using UnityEngine;
using System.Collections;
using DanmakU;

public class TestBehaviorEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab bulletPrefab;
    private FireBuilder fireData;

    [SerializeField]
    private float fireRate = 1;
    private float fireDelay = 0;

    void Start()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        fireData = new FireBuilder(bulletPrefab, Field);
        fireData.From(transform);
        fireData.Towards(player);
        fireData.WithSpeed(10);

        AddMovementBehavior(new FollowPlayerConstantSpeedBehavior(5, 5, 3));
        AddMovementBehavior(new IdleMovementBehavior(3));
        AddAttackBehavior(new CircularAttackBehavior(fireData, 1, 16, 3));
        AddAttackBehavior(new ConstantAttackBehavior(fireData, 16, 3));
        loopBehaviors = true;
    }
    
    public override void NormalUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
