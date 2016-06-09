using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public abstract class Enemy : DanmakuCollider, IPausable
{
    protected Player player;
    protected List<Enemy> enemies;

    private Bounds2D bounds;

    [SerializeField]
    protected int maxHealth;
    protected int health;

    public DanmakuField Field
    {
        get;
        set;
    }

    public bool Paused
    {
        get;
        set;
    }

    protected override void DanmakuCollision(Danmaku danmaku, RaycastHit2D info)
    {
        health -= danmaku.Damage;
        danmaku.Deactivate();

        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Start()
    {
        player = ((GameController)GameController.Instance).Player;
        enemies = EnemyManager.Instance.Enemies;

        bounds = new Bounds2D(GetComponent<Collider2D>().bounds);

        health = maxHealth;
    }

    public virtual void Die()
    {
        // TODO Death animation
        enemies.Remove(this);
        Destroy(gameObject);
    }
}
