using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;
using DanmakU.Modifiers;
using System.Collections.Generic;

public class CircleAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private FireBuilder fireData;

    private DynamicFloat fireSpeed;
    private DynamicFloat fireRate;
    private DynamicFloat range;
    private DynamicInt count;
    private DynamicFloat deltaSpeed;
    private DynamicFloat deltaAngularSpeed;
    private float fireDelay;

    private Color color;

    private List<DanmakuModifier> modifiers;
    private List<IDanmakuController> controllers;

    public CircleAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicFloat range, DynamicInt count, DynamicFloat deltaSpeed, DynamicFloat deltaAngularSpeed, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        this.range = range;
        this.count = count;
        this.deltaSpeed = deltaSpeed;
        this.deltaAngularSpeed = deltaAngularSpeed;
        color = bullet.Color;

        modifiers = new List<DanmakuModifier>();
        controllers = new List<IDanmakuController>();
    }

    public CircleAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicFloat range, DynamicInt count, float duration) : this(bullet, fireSpeed, fireRate, range, count, 0, 0, duration) { }

    public CircleAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicInt count, float duration) : this(bullet, fireSpeed, fireRate, (count - 1) * 360 / count, count, 0, 0, duration) {}

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        fireDelay = 0;

        this.fireData = new FireBuilder(bullet, enemy.Field);
        fireData.From(enemy);
        fireData.Towards(player);
        fireData.WithSpeed(fireSpeed);
        
        CircularBurstModifier cbm = new CircularBurstModifier(range, count, deltaSpeed, deltaAngularSpeed);
        fireData.WithModifier(cbm);
    }

    public override void Update()
    {
        base.Update();

        if (fireDelay <= 0)
        {
            fireDelay = 1 / fireRate;
            fireData.Fire();
        }
        fireDelay -= Time.deltaTime;
    }

    public CircleAttackBehavior SetColor(Color color)
    {
        this.color = color;
        return this;
    }

    public CircleAttackBehavior AddModifier(DanmakuModifier modifier)
    {
        modifiers.Add(modifier);
        return this;
    }

    public CircleAttackBehavior AddController(IDanmakuController controller)
    {
        controllers.Add(controller);
        return this;
    }
}
