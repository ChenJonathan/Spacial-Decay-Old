using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

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

        List<AttackBehavior> attacks = new List<AttackBehavior>();
        AddMovementBehavior(new OrbitPlayerConstantSpeedBehavior(5, 8, float.MaxValue));
        //AddMovementBehavior(new IdleMovementBehavior(3));
        attacks.Add(new CircularAttackBehavior(fireData, 1, 16, 6));
        attacks.Add(new ConstantAttackBehavior(fireData, 8, 6));
        AddAttackBehavior(new CombinedAttackBehavior(attacks));
        loopBehaviors = true;
    }
    
    public override void NormalUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
