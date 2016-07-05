using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;
using System.Collections.Generic;

public class ConstantAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private FireBuilder fireData;

    private Vector2? target;

    private DynamicFloat fireSpeed;
    private DynamicFloat fireRate;

    private DynamicFloat rotateSpeed;

    private float fireDelay;
    private Color color;

    public ConstantAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, Color color, float duration) : base(duration)
    {
        this.bullet = bullet;
        target = null;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        rotateSpeed = 0f;
        this.color = color;
    }

    public ConstantAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicFloat rotateSpeed, Color color, float duration) : base(duration)
    {
        this.bullet = bullet;
        target = null;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        this.rotateSpeed = rotateSpeed;
        this.color = color;
    }

    public ConstantAttackBehavior(DanmakuPrefab bullet, Vector2 target, DynamicFloat fireSpeed, DynamicFloat fireRate, Color color, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.target = target;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        rotateSpeed = 0f;
        this.color = color;
    }

    public ConstantAttackBehavior(DanmakuPrefab bullet, Vector2 target, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicFloat rotateSpeed, Color color, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.target = target;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        this.rotateSpeed = rotateSpeed;
        this.color = color;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        fireDelay = 0;

        this.fireData = new FireBuilder(bullet, enemy.Field);
        fireData.From(enemy);
        if (target == null)
            fireData.Towards(player);
        else
            fireData.Towards((Vector2)target);
        fireData.WithSpeed(fireSpeed);
        
        RotateController rc = new RotateController();
        rc.RotateSpeed = rotateSpeed;
       
        ColorChangeController cc = new ColorChangeController();
        Gradient g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[1];
        GradientAlphaKey[] gak = new GradientAlphaKey[1];
        gck[0].color = Color.white;
        gak[0].alpha = 1;
        g.SetKeys(gck, gak);
        cc.ColorGradient = g;
        
        fireData.WithController(rc);
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
