using UnityEngine;
using System.Collections;
using DanmakU;
using System;

public class Player : DanmakuCollider, IPausable
{

    [SerializeField]
    private DanmakuPrefab bulletPrefab;
    [SerializeField]
    private GameObject fireTargetPrefab;
    [SerializeField]
    private GameObject moveTargetPrefab;

    private Collider2D collider;
    private FireBuilder fireData;

    private GameObject fireCrosshair;
    private GameObject moveCrosshair;
    private GameObject counter;
    private LivesCounter livesCounter;

    [SerializeField]
    private int lives = 5;
    public int Lives
    {
        get
        {
            return lives;
        }
    }

    private bool invincible = false;
    private static readonly float INVINCIBILITY_ON_SPAWN = 5;
    private static readonly float INVINCIBILITY_ON_HIT = 3;

    [SerializeField]
    private float fireRate = 12;
    private float fireDelay = 0;
    [SerializeField]
    private float moveSpeed = 16;
    [SerializeField]
    private float rotateSpeed = 10;
    
    private Vector2 fireTarget;
    private Renderer fireTargetRenderer;
    private Vector2 moveTarget;
    private Renderer moveTargetRenderer;

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
        danmaku.Deactivate();
        if(!invincible)
        {
            lives--;
            livesCounter.UpdateCounter(lives);
            StartCoroutine(setInvincible(INVINCIBILITY_ON_HIT));
        }
    }

    public override void Awake()
    {
        base.Awake();

        collider = GetComponent<Collider2D>();
        fireTarget = new Vector2(transform.position.x, transform.position.y + 0.001f);
        moveTarget = new Vector2(transform.position.x, transform.position.y);

        fireCrosshair = (GameObject)Instantiate(fireTargetPrefab, fireTarget, Quaternion.identity);
        fireCrosshair.transform.parent = GameController.Instance.transform;
        fireTargetRenderer = fireCrosshair.GetComponent<Renderer>();
        moveCrosshair = (GameObject)Instantiate(moveTargetPrefab, moveTarget, Quaternion.identity);
        moveCrosshair.transform.parent = GameController.Instance.transform;
        moveTargetRenderer = moveCrosshair.GetComponent<Renderer>();

        counter = GameObject.FindGameObjectWithTag("Counter");
        livesCounter = counter.GetComponent<LivesCounter>();
    }

    public void Start()
    {
        fireData = new FireBuilder(bulletPrefab, Field);
        fireData.From(transform);
        fireData.Towards(fireTarget);
        fireData.WithSpeed(32, 48);
        fireData.WithRotation(-4, 4);
        fireData.WithDamage(4, 8);

        StartCoroutine(setInvincible(INVINCIBILITY_ON_SPAWN));
    }
	
	void Update()
    {
	    if(!Paused)
        {
            HandleInput();

            if(isMoving())
            {
                moveTargetRenderer.enabled = true;
            }
            else
            {
                fireDelay -= Time.deltaTime;
                moveTargetRenderer.enabled = false;
            }

            // Attack
            if(fireDelay <= 0)
            {
                fireDelay += 1 / fireRate;
                fireData.Fire();
            }
        }
	}

    void FixedUpdate()
    {
        if(!Paused)
        {
            transform.position = Vector2.Lerp(transform.position, moveTarget, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, (Vector3)fireTarget - transform.position), Time.deltaTime * rotateSpeed);
        }
    }

    private void HandleInput()
    {
        if(Input.GetMouseButton(0))
        {
            SetMoveTarget(Field.WorldPoint(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height)));
        }
        if(Input.GetMouseButton(1))
        {
            SetFireTarget(Field.WorldPoint(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height)));
        }
    }

    public void SetFireTarget(Vector2 target)
    {
        fireCrosshair.transform.position = fireTarget;
        fireTarget = target;
        fireData.Towards(fireTarget);
    }

    public void SetMoveTarget(Vector2 target)
    {
        moveCrosshair.transform.position = target;
        moveTarget = BoundsUtil.VerifyBounds(target, new Bounds2D(collider.bounds), Field.MovementBounds);
    }

    public bool isMoving()
    {
        return Vector2.Distance(transform.position, moveTarget) > 0.01;
    }

    private bool isPaused()
    {
        return Paused;
    }

    public IEnumerator setInvincible(float time)
    {
        Renderer renderer = GetComponent<Renderer>();
        Color color = renderer.material.color;
        invincible = true;
        for (float i = 0; i < time; i += Time.deltaTime + 0.05f)
        {
            color.a = 1.25f - color.a;
            renderer.material.color = color;
            yield return new WaitWhile(isPaused);
            yield return new WaitForSeconds(0.05f);
        }
        color.a = 1;
        renderer.material.color = color;
        invincible = false;
        yield break;
    }
}
