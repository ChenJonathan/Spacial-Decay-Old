using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;
using DanmakU.Modifiers;

public class TestModifierEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab straightBulletPrefab;
    [SerializeField]
    private DanmakuPrefab circleBulletPrefab;

    void Start()
    {
        health = maxHealth = 100 * difficulty;
        
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        FireBuilder straightFireData = new FireBuilder(straightBulletPrefab, Field);
        straightFireData.From(transform);
        straightFireData.Towards(player);
        straightFireData.WithSpeed(4);
        straightFireData.WithRotation(-180, 180);
        straightFireData.WithModifier(new CircularBurstModifier(8, 8, 0, 2));

        FireBuilder circleFireData = new FireBuilder(circleBulletPrefab, Field);
        circleFireData.From(transform);
        circleFireData.Towards(player);

        AddAttackBehavior(new ConstantAttackBehavior(straightFireData, 4, 6));
        AddMovementBehavior(new FollowPlayerConstantSpeedBehavior(5, 5, 3));
        AddMovementBehavior(new IdleMovementBehavior(3));
        LoopBehaviors = true;
    }
    
    public override void NormalUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
