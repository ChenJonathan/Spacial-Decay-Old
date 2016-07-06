using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;
using System.Collections.Generic;

public class ConstantAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private Vector2? target;
    private FireBuilder fireData;

    private DynamicFloat fireSpeed;
    private DynamicFloat fireRate;

    private DynamicFloat rotateSpeed;

    private Color color;
    private List<DanmakuModifier> modifiers;
    private List<IDanmakuController> controllers;

    private float fireDelay;

    public ConstantAttackBehavior(DanmakuPrefab bullet, Vector2? target, DynamicFloat fireSpeed, DynamicFloat fireRate, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.target = target;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        color = Color.white;

        modifiers = new List<DanmakuModifier>();
        controllers = new List<IDanmakuController>();
}
    
    public ConstantAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, float duration) : this(bullet, null, fireSpeed, fireRate, duration) { }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        fireDelay = 0;

        fireData = new FireBuilder(bullet, enemy.Field);
        fireData.From(enemy);
        if (target.HasValue)
            fireData.Towards((Vector2)target);
        else
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
        fireData.WithModifiers(modifiers);
        fireData.WithController(controllers);
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

    public ConstantAttackBehavior SetColor(Color color)
    {
        this.color = color;
        return this;
    }

    public ConstantAttackBehavior AddModifier(DanmakuModifier modifier)
    {
        modifiers.Add(modifier);
        return this;
    }

    public ConstantAttackBehavior AddController(IDanmakuController controller)
    {
        controllers.Add(controller);
        return this;
    }
}
