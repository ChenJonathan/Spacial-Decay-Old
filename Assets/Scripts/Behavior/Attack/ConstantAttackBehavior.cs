using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;

public class ConstantAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private FireBuilder fireData;

    private DynamicFloat fireSpeed;
    private DynamicFloat fireRate;
    private float fireDelay;
    private Color color;

    public ConstantAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, float duration, Color color) : base(duration)
    {
        this.bullet = bullet;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        this.color = color;
    }

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
