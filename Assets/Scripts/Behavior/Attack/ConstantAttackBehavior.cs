using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;
using System.Collections.Generic;

public class ConstantAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private bool targetPlayer, trackPlayer;
    private Vector2 target;
    private FireBuilder fireData;

    private DynamicFloat fireSpeed;
    private DynamicFloat fireRate;

    private DynamicFloat rotateSpeed;

    private float fireDelay;
    private Color color;

    public ConstantAttackBehavior(DanmakuPrefab bullet, bool trackPlayer, DynamicFloat fireSpeed, DynamicFloat fireRate, Color color, float duration) : this(bullet, Vector2.zero, fireSpeed, fireRate, color, duration)
    {
        targetPlayer = true;
        this.trackPlayer = trackPlayer;
    }

    public ConstantAttackBehavior(DanmakuPrefab bullet, Vector2 target, DynamicFloat fireSpeed, DynamicFloat fireRate, Color color, float duration) : base(duration)
    {
        targetPlayer = trackPlayer = false;
        this.bullet = bullet;
        this.target = target;
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
        if (trackPlayer)
            fireData.Towards(player);
        else
        {
            if (targetPlayer)
                target = player.transform.position;
            fireData.Towards(target);
        }
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
