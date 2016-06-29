using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public class TestBehaviorEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab straightBulletPrefab;
    [SerializeField]
    private DanmakuPrefab circleBulletPrefab;
    private FireBuilder straightFireData;
    private FireBuilder circleFireData;

    [SerializeField]
    private float fireRate = 1;
    private float fireDelay = 0;

    void Start()
    {
        health = maxHealth = 100 * difficulty;
        
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        straightFireData = new FireBuilder(straightBulletPrefab, Field);
        straightFireData.From(transform);
        straightFireData.Towards(player);
        straightFireData.WithSpeed(10);

        circleFireData = new FireBuilder(circleBulletPrefab, Field);
        circleFireData.From(transform);
        circleFireData.Towards(player);
        circleFireData.WithSpeed(7);

        List<AttackBehavior> attacks = new List<AttackBehavior>();
        AddMovementBehavior(new OrbitAroundPlayerBehavior(1, 4, 8, float.MaxValue));
        // AddMovementBehavior(new IdleMovementBehavior(3));
        attacks.Add(new CircularAttackBehavior(circleFireData, 2, 16, 6));
        attacks.Add(new ConstantAttackBehavior(straightFireData, 8, 6));
        AddAttackBehavior(new CombinedAttackBehavior(attacks));
        loopBehaviors = true;
    }
    
    public override void NormalUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
