using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;
using DanmakU.Modifiers;

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

    public CircleAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicFloat range, DynamicInt count, DynamicFloat deltaSpeed, DynamicFloat deltaAngularSpeed, Color color, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        this.range = range;
        this.count = count;
        this.deltaSpeed = deltaSpeed;
        this.deltaAngularSpeed = deltaAngularSpeed;
        this.color = color;
    }

    public CircleAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicInt count, Color color, float duration) : this(bullet, fireSpeed, fireRate, (count - 1) * 360 / count, count, 0, 0, color, duration) {}

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        fireDelay = 0;

        this.fireData = new FireBuilder(bullet, enemy.Field);
        fireData.From(enemy);
        fireData.Towards(player);
        fireData.WithSpeed(fireSpeed);

        ColorChangeController cc = new ColorChangeController();
        Gradient g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[1];
        GradientAlphaKey[] gak = new GradientAlphaKey[1];
        gck[0].color = color;
        gak[0].alpha = 1;
        g.SetKeys(gck, gak);
        cc.ColorGradient = g;
        fireData.WithController(cc);

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
}
