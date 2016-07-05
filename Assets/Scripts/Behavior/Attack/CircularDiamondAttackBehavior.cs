using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Modifiers;

public class CircularDiamondAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private FireBuilder fireData;

    private Vector2? target;
    private DynamicFloat fireSpeed;
    private DynamicFloat angle;
    private DynamicInt diamondSize;

    private CircularBurstModifier modifier;
    private bool increasingSize;
    private float fireDelay;

    public CircularDiamondAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat angle, DynamicInt diamondSize, float duration) : this(bullet, null, fireSpeed, angle, diamondSize, duration) { }

    public CircularDiamondAttackBehavior(DanmakuPrefab bullet, Vector2? target, DynamicFloat fireSpeed, DynamicFloat angle, DynamicInt diamondSize, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.target = target;
        this.fireSpeed = fireSpeed;
        this.angle = angle;
        this.diamondSize = diamondSize;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        increasingSize = true;
        fireDelay = 0;

        this.fireData = new FireBuilder(bullet, enemy.Field);
        fireData.From(enemy);
        if (target.HasValue)
            fireData.Towards((Vector2)target);
        else
            fireData.Towards(player.transform.position);
        fireData.WithSpeed(fireSpeed);
        modifier = new CircularBurstModifier(0, 1, 0, 0);
        fireData.WithModifier(modifier);
    }

    public override void Update()
    {
        base.Update();

        if (fireDelay <= 0)
        {
            fireDelay = duration / diamondSize;
            fireData.Fire();
            if (modifier.Count == diamondSize)
                increasingSize = false;
            if (increasingSize)
            {
                modifier.Count += 1;
                modifier.Range += angle;
            }
            else
            {
                modifier.Count -= 1;
                modifier.Range -= angle;
            }
        }
        fireDelay -= Time.deltaTime;
    }
}
