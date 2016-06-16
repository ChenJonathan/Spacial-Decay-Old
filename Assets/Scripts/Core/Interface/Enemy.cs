using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public abstract partial class Enemy : DanmakuCollider, IPausable
{
    private Wave wave;
    public Wave Wave
    {
        get
        {
            return wave;
        }
        set
        {
            wave = value;
            player = wave.Player;
            enemies = wave.Enemies;
        }
    }

    protected Player player;
    protected List<Enemy> enemies;

    [SerializeField]
    private int maxHealth;
    private int health;

    private Bounds2D bounds;

    private GameObject healthBar;
    [SerializeField]
    private float healthBarSize = 1.0f;

    [SerializeField]
    private GameObject DamageGUI;
    [SerializeField]
    private GameObject HealthBar;

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
        
        GameObject damageGUI = (GameObject) Instantiate(DamageGUI, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
        damageGUI.transform.parent = Field.transform;
        damageGUI.GetComponent<TextMesh>().text = "" + danmaku.Damage;

        float healthProportion = 1.0f * health / maxHealth;
        healthBar.GetComponentInChildren<HealthIndicator>().Activate(healthProportion);

        if(health <= 0)
        {
            Die();
        }
    }

    public override void Awake()
    {
        base.Awake();
        bounds = new Bounds2D(GetComponent<Collider2D>().bounds);

        healthBar = (GameObject)Instantiate(HealthBar, transform.position, Quaternion.identity);
        healthBar.transform.parent = transform;
        healthBar.transform.localScale = new Vector3(healthBarSize, 1, 1);

        health = maxHealth;
    }

    public virtual void FixedUpdate()
    {
        if(attackBehavior != null)
            attackBehavior.Update();
        if(movementBehavior != null)
            movementBehavior.Update();
    }

    public virtual void Die()
    {
        Wave.OnEnemyDeath(this);
    }
}
