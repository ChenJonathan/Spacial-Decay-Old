using UnityEngine;
using System.Collections;
using DanmakU;
using System;

public class Player : DanmakuCollider, IPausable
{

    [SerializeField]
    private DanmakuPrefab prefab;
    private FireBuilder fireData;

    [SerializeField]
    private int lives = 5;

    [SerializeField]
    private float fireRate = 12;
    private float fireDelay = 0;
    [SerializeField]
    private float moveSpeed = 16;
    [SerializeField]
    private float rotateSpeed = 8;

    private Vector2 fireTarget;
    private Vector2 moveTarget;

    public virtual DanmakuField Field
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

    void Start ()
    {
        fireTarget = new Vector2(transform.position.x, transform.position.y + 10);
        moveTarget = new Vector2(transform.position.x, transform.position.y);
        
        fireData = new FireBuilder(prefab, Field);
        fireData.From(transform);
        fireData.Towards(fireTarget);
        fireData.WithSpeed(32, 48);
        fireData.WithRotation(-8, 8);
    }
	
	void Update ()
    {
	    if(!Paused)
        {
            HandleInput();

            transform.position = Vector2.Lerp(transform.position, moveTarget, Time.deltaTime * moveSpeed);

            if(Vector2.Distance((Vector2)transform.position, moveTarget) < 0.1)
            {
                fireDelay -= Time.deltaTime;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.forward, fireTarget), Time.deltaTime * rotateSpeed);
            }
            else
            {
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.forward, moveTarget), Time.deltaTime * rotateSpeed);
            }
            if (fireDelay <= 0)
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
            moveTarget = Field.WorldPoint(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));
        }
        if(Input.GetMouseButton(1))
        {
            fireTarget = Field.WorldPoint(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));
            fireData.Towards(fireTarget);
        }
    }
}
