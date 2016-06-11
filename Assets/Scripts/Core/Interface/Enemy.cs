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
    private int maxHealth;
    private int health;
    
    private GameObject healthBar;

    [SerializeField]
    protected GameObject DamageGUI;
    [SerializeField]
    protected GameObject HealthBar;

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

    public virtual void Start()
    {
        player = ((GameController)GameController.Instance).Player;
        enemies = EnemyManager.Instance.Enemies;

        bounds = new Bounds2D(GetComponent<Collider2D>().bounds);
        healthBar = (GameObject)Instantiate(HealthBar, transform.position, Quaternion.identity);
        healthBar.transform.parent = transform;

        health = maxHealth;
    }

    public virtual void Die()
    {
        // TODO Death animation
        enemies.Remove(this);
        Destroy(gameObject);
    }
}
