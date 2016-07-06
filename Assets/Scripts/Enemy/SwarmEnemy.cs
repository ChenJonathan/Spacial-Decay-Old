using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;
using DanmakU.Modifiers;

public class SwarmEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab bulletPrefab;
    private FireBuilder fireData;

    void Start()
    {
        Health = MaxHealth = 100;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        fireData = new FireBuilder(bulletPrefab, Field);
        fireData.From(transform);
        fireData.WithSpeed(4);
        fireData.WithAngularSpeed(270);
        fireData.WithModifier(new CircularBurstModifier(288, 5, 0, 0));
        fireData.WithController(new AutoDeactivateController(80));

        AddAttackBehavior(new IdleAttackBehavior(3));
        AddAttackBehavior(new ConstantAttackBehavior(bulletPrefab, true, 8, 12, Color.cyan, 1));
        AddAttackBehavior(new IdleAttackBehavior(2));
        AddMovementBehavior(new FollowPlayerBehavior(8, -2, 2, 3));
        AddMovementBehavior(new IdleMovementBehavior(3));
        LoopBehaviors = true;
    }

    public override void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
