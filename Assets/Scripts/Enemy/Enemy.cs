using UnityEngine;
using System.Collections;
using DanmakU;

public abstract class Enemy : DanmakuCollider, IPausable
{
    protected Bounds2D bounds;
    protected Player player;

    [SerializeField]
    private int maxHealth;
    private int health;

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

        if(health < 0)
        {
            // TODO Death animation
            Destroy(gameObject);
        }
    }

    public virtual void Start ()
    {
        bounds = new Bounds2D(GetComponent<Collider2D>().bounds);
        player = ((GameController)GameController.Instance).Player;

        health = maxHealth;
    }
}
