using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public partial class Enemy : DanmakuCollider, IPausable
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

    [HideInInspector]
    public int MaxHealth;
    [HideInInspector]
    public int Health;

    [HideInInspector]
    public bool FacePlayer;

    private Bounds2D bounds;
    private GameObject healthBar;
    private float healthBarSize = 1.0f;

    [SerializeField]
    private GameObject damageGUIPrefab;
    [SerializeField]
    private GameObject healthBarPrefab;

    [SerializeField]
    private Color color;

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

    public override sealed void Awake()
    {
        base.Awake();
        bounds = new Bounds2D(GetComponent<Collider2D>().bounds);
        TagFilter = "Friendly";

        healthBar = (GameObject)Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBar.transform.parent = transform;
        healthBar.transform.localScale = new Vector3(healthBarSize, 1, 1);
    }

    public void Update()
    {
        if (!Paused)
        {
            if (attackBehavior != null)
                attackBehavior.Update();
            if (movementBehavior != null)
                movementBehavior.Update();

            NormalUpdate();
        }
    }

    public virtual void NormalUpdate() { }

    public void FixedUpdate()
    {
        if (!Paused)
        {
            if (attackBehavior != null)
                attackBehavior.FixedUpdate();
            if (movementBehavior != null)
                movementBehavior.FixedUpdate();

            if (FacePlayer)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);

            NormalFixedUpdate();
        }
    }

    public virtual void NormalFixedUpdate() { }

    public virtual void Die()
    {
        Wave.OnEnemyDeath(this);
    }

    protected override void DanmakuCollision(Danmaku danmaku, RaycastHit2D info)
    {
        Health -= danmaku.Damage;
        danmaku.Deactivate();

        /*
        GameObject damageGUI = (GameObject) Instantiate(damageGUIPrefab, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
        damageGUI.transform.parent = Field.transform;
        damageGUI.GetComponent<TextMesh>().text = "" + danmaku.Damage;
        */

        float healthProportion = (float)Health / MaxHealth;
        healthBar.GetComponentInChildren<HealthIndicator>().Activate(healthProportion);

        if (Health <= 0)
        {
            Die();
        }
    }
}
