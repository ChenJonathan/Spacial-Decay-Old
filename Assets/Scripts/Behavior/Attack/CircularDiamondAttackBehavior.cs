using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Modifiers;
using System.Collections.Generic;

public class CircularDiamondAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private FireBuilder fireData;

    private bool targetPlayer;
    private Vector2 target;
    private DynamicFloat fireSpeed;
    private DynamicFloat angle;
    private DynamicInt diamondSize;

    private CircularBurstModifier modifier;
    private bool increasingSize;
    private float fireDelay;

    private Color color;

    private List<DanmakuModifier> modifiers;
    private List<IDanmakuController> controllers;

    public CircularDiamondAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat angle, DynamicInt diamondSize, float duration) : this(bullet, Vector2.zero, fireSpeed, angle, diamondSize, duration)
    {
        targetPlayer = true;
    }

    public CircularDiamondAttackBehavior(DanmakuPrefab bullet, Vector2 target, DynamicFloat fireSpeed, DynamicFloat angle, DynamicInt diamondSize, float duration) : base(duration)
    {
        this.bullet = bullet;
        targetPlayer = false;
        this.target = target;
        this.fireSpeed = fireSpeed;
        this.angle = angle;
        this.diamondSize = diamondSize;
        color = bullet.Color;

        modifiers = new List<DanmakuModifier>();
        controllers = new List<IDanmakuController>();
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        increasingSize = true;
        fireDelay = 0;

        this.fireData = new FireBuilder(bullet, enemy.Field);
        fireData.From(enemy);
        if (targetPlayer)
            target = player.transform.position;
        fireData.Towards(target);
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


    public CircularDiamondAttackBehavior SetColor(Color color)
    {
        this.color = color;
        return this;
    }

    public CircularDiamondAttackBehavior AddModifier(DanmakuModifier modifier)
    {
        modifiers.Add(modifier);
        return this;
    }

    public CircularDiamondAttackBehavior AddController(IDanmakuController controller)
    {
        controllers.Add(controller);
        return this;
    }
}
