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

    [SerializeField]
    private int lives = 5;

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
        lives--;
        danmaku.Deactivate();
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        fireTarget = new Vector2(transform.position.x, transform.position.y + 5);
        moveTarget = new Vector2(transform.position.x, transform.position.y);

        fireCrosshair = (GameObject)Instantiate(fireTargetPrefab, fireTarget, Quaternion.identity);
        fireCrosshair.transform.parent = Field.transform;
        fireTargetRenderer = fireCrosshair.GetComponent<Renderer>();
        moveCrosshair = (GameObject)Instantiate(moveTargetPrefab, moveTarget, Quaternion.identity);
        moveCrosshair.transform.parent = Field.transform;
        moveTargetRenderer = moveCrosshair.GetComponent<Renderer>();

        fireData = new FireBuilder(bulletPrefab, Field);
        fireData.From(transform);
        fireData.Towards(fireTarget);
        fireData.WithSpeed(32, 48);
        fireData.WithRotation(-8, 8);
        fireData.WithDamage(4, 8);
    }
	
	void Update()
    {
	    if(!Paused)
        {
            HandleInput();

            transform.position = Vector2.Lerp(transform.position, moveTarget, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, (Vector3)fireTarget - transform.position), Time.deltaTime * rotateSpeed);

            if(isMoving())
            {
                moveTargetRenderer.enabled = true;
            }
            else
            {
                fireDelay -= Time.deltaTime;
                moveTargetRenderer.enabled = false;
            }

            if(fireDelay <= 0)
            {
                fireDelay += 1 / fireRate;
                fireData.Fire();
            }
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
}
