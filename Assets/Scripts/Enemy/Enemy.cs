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

    [SerializeField]
    protected GameObject DamageGUI;
    private GUIController guiController;

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
        //damageGUI.transform.parent = Field.transform; // For making the GUI child of the field
        damageGUI.GetComponent<TextMesh>().text = "" + danmaku.Damage;
        
        // May work on making GUIController in the future
        //guiController.GenerateDamageIndicator(danmaku.Damage, new Vector2(transform.position.x, transform.position.y + 2));
        
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
    }
}
