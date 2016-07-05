using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Controllers;

public class ConstantAttackBehavior : Enemy.AttackBehavior
{
    private DanmakuPrefab bullet;
    private FireBuilder fireData;

    private Vector2? target;

    private DynamicFloat fireSpeed;
    private DynamicFloat fireRate;

    private DynamicFloat rotateSpeed;

    private float fireDelay;

    public ConstantAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, float duration) : base(duration)
    {
        this.bullet = bullet;
        target = null;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        rotateSpeed = 0f;
    }

    public ConstantAttackBehavior(DanmakuPrefab bullet, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicFloat rotateSpeed, float duration) : base(duration)
    {
        this.bullet = bullet;
        target = null;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        this.rotateSpeed = rotateSpeed;
    }

    public ConstantAttackBehavior(DanmakuPrefab bullet, Vector2 target, DynamicFloat fireSpeed, DynamicFloat fireRate, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.target = target;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        rotateSpeed = 0f;
    }

    public ConstantAttackBehavior(DanmakuPrefab bullet, Vector2 target, DynamicFloat fireSpeed, DynamicFloat fireRate, DynamicFloat rotateSpeed, float duration) : base(duration)
    {
        this.bullet = bullet;
        this.target = target;
        this.fireSpeed = fireSpeed;
        this.fireRate = fireRate;
        this.rotateSpeed = rotateSpeed;
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

        RotateController controller = new RotateController();
        controller.RotateSpeed = rotateSpeed;

        fireData.WithController(controller);
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
