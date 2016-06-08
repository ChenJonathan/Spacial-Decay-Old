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
            // TODO Death animation
            Destroy(gameObject);
        }
    }
    
    public virtual void Start ()
    {
        bounds = new Bounds2D(GetComponent<Collider2D>().bounds);
        player = ((GameController)GameController.Instance).Player;
        health = maxHealth;

        healthBar = (GameObject)Instantiate(HealthBar, transform.position, Quaternion.identity);
        healthBar.transform.parent = transform;
    }
}
